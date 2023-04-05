namespace Fwsh.WebApi.Controllers.Customer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.Requests.Customer;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("customer/orders/production")]
public class ProductionOrderController : FwshController
{
    const int PAGESIZE = 10;

    public ProductionOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected IActionResult GetOrderList (int? page, Expression<Func<ProductionOrder, bool>> condition)
    {
        if (page == null) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<ProductionOrder> orders = dataContext.ProductionOrders
            .Include(order => order.Design)
            .Where(order => order.CustomerId == user.ConfirmedId)
            .Where(condition);

        return Ok ( orders.OrderBy(order => order.Id).Paginate (
            (int)page, PAGESIZE, order => new ProductionOrderResult(order).Mini()
        ));
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null)
    {
        return GetOrderList (page, order => 
               order.Status == OrderStatus.Unknown
            || order.Status == OrderStatus.Submitted 
            || order.Status == OrderStatus.Working 
            || order.Status == OrderStatus.Delayed 
            || order.Status == OrderStatus.Finished
        );
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null)
    {
        return GetOrderList (page, order => 
               order.Status == OrderStatus.ReceivedAndPaid
            || order.Status == OrderStatus.Rejected 
            || order.Status == OrderStatus.Impossible
        );
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id) 
    {
        var order = dataContext.ProductionOrders
            .Include(order => order.Fabric.FabricType)
            .Include(order => order.Fabric.Color)
            .Include(order => order.DecorMaterial.Color)
            .Include(order => order.Customer)
            .Include(order => order.Notifications)
            .Include(order => order.Design)
            .ThenInclude(design => design.Photos)
            .Where(order => order.Id == id && order.CustomerId == user.ConfirmedId)
            .FirstOrDefault();

        if (order == null) {
            return NotFound (new BadFieldResult("id"));
        }

        return Ok (new ProductionOrderResult(order).ForCustomer());
    }

    [HttpPost("create")]
    public IActionResult Create (ProductionOrderRequest request)
    {
        var order = new ProductionOrder() {
            Status = OrderStatus.Unknown,
            CustomerId = user.ConfirmedId
        };

        var result = this.OnApplyRequest(order, request);

        if (result != null) return result;

        try {
            dataContext.Add(order);
            dataContext.SaveChanges();
            return Ok (new CreationResult(order.Id, "Successfully created new Production Order"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to create Production Order"));
        }
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, ProductionOrderRequest request)
    {
        var order = dataContext.ProductionOrders
            .FirstOrDefault(order => order.Id == id && order.CustomerId == user.ConfirmedId);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        bool canUpdate = order.Status == OrderStatus.Unknown;

        if (! canUpdate) {
            return BadRequest(new FailResult($"Can not update Production Order {id} with status '{order.Status}'"));
        }

        var result = this.OnApplyRequest(order, request);

        if (result != null) return result;

        try {
            dataContext.Update(order);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Successfully updated Production Order {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to update Production Order"));
        }
    }

    protected IActionResult OnApplyRequest (ProductionOrder order, ProductionOrderRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest (new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest (new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        var design = dataContext.Designs.Find(request.DesignId);

        if (design == null) {
            return BadRequest (new BadFieldResult("designId"));
        }

        var fabric = dataContext.Fabrics.Find(request.FabricId);

        if (fabric == null) {
            return BadRequest (new BadFieldResult("fabricId"));
        }

        var decorMaterial = dataContext.Materials
            .FirstOrDefault(m => m.IsDecorative && m.Id == request.DecorMaterialId);

        if (decorMaterial == null && design.DecorMaterialQuantity > 0) {
            return BadRequest (new BadFieldResult("decorMaterialId"));
        }
        if (decorMaterial != null && ! decorMaterial.IsDecorative) {
            return BadRequest (new BadFieldResult("decorMaterialId"));
        }

        var customer = dataContext.Customers
            .Include(customer => customer.ProductionOrders)
            .FirstOrDefault(customer => customer.Id == user.ConfirmedId);

        customer.UpdateDiscountPercent();

        int fixedDesignPrice = (int)(design.Price 
            + fabric.PricePerUnit * design.FabricQuantity 
            + (decorMaterial?.PricePerUnit ?? 0) * design.DecorMaterialQuantity);

        request.ApplyTo(order);

        order.DecorMaterialId = decorMaterial?.Id;
        order.PricePerOne = fixedDesignPrice.WithDiscountFor(customer);

        return null; // null means nothing gone wrong
    }

    [HttpPost("confirm-submit/{id}")]
    public IActionResult ConfirmSubmit (int id)
    {
        var order = dataContext.ProductionOrders
            .FirstOrDefault(order => order.Id == id && order.CustomerId == user.ConfirmedId);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        if (order.Status == OrderStatus.Unknown) {
            order.Status = OrderStatus.Submitted;
            dataContext.ProductionOrders.Update(order);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult (
                $"Successfully confirmed submission of Production Order {id}"
            ));
        }
        else {
            return BadRequest ( new FailResult (
                $"Can not confirm submission of Production Order {id} with status {order.Status}"
            ));
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete (int id)
    {
        var order = dataContext.ProductionOrders
            .Where(order => order.Id == id && order.CustomerId == user.ConfirmedId)
            .FirstOrDefault();

        if (order == null) {
            return NotFound (new BadFieldResult("id"));
        }

        bool canSafelyDelete = order.Status == OrderStatus.Submitted 
                            || order.Status == OrderStatus.Unknown;

        if (! canSafelyDelete) {
            return BadRequest (new FailResult("Order status does not allow deletion"));
        }

        try {
            dataContext.Remove(order);
            dataContext.SaveChanges();
            return Ok (new DeletionResult(order.Id, "Successfully deleted Production Order"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong"));
        }
    }

    [HttpPost("read-notifications")]
    public IActionResult ReadNotifications (int? order = null, int? id = null, int? last = null)
    {
        IQueryable<ProductionNotification> notifications = dataContext.ProductionOrders
            .Include(n => n.Notifications)
            .Where(order => order.CustomerId == user.ConfirmedId)
            .SelectMany(order => order.Notifications)
            .Where(n => n.IsRead == false);

        if (order is int orderId) {
            notifications = notifications.Where(n => n.ProductionOrderId == orderId);
            if (id is int notificationId) {
                notifications = notifications.Where(n => n.Id == notificationId);    
            }
            else if (last is int lastNotificationId) {
                notifications = notifications.Where(n => n.Id <= lastNotificationId);
            }
            else {
                return BadRequest (new BadFieldResult("id", "last"));
            }
        }
        else {
            return BadRequest (new BadFieldResult("order"));
        }
        
        try {
            var selectedNotifications = notifications.ToList();
            foreach (var n in selectedNotifications) n.IsRead = true;
            dataContext.ProductionNotifications.UpdateRange(selectedNotifications);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult (
                $"Production order {order}: Notification " 
                + (id?.ToString() ?? $"until {last}")
                + " marked as read"
            ));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong"));
        }
    }
}
