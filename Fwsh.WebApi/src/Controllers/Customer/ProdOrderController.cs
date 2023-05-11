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
public class ProdOrderController : FwshController
{
    const int PAGESIZE = 10;

    public ProdOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected IActionResult GetOrderList (int? page, Expression<Func<ProdOrder, bool>> condition)
    {
        if (page == null) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<ProdOrder> orders = dataContext.ProdOrders
            .Include(order => order.Design)
            .Where(order => order.CustomerId == user.ConfirmedId)
            .Where(condition);

        return Ok ( orders.OrderByDescending(order => order.Id).Paginate (
            (int)page, PAGESIZE, order => new ProdOrderResult(order).Mini()
        ));
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null)
    {
        return GetOrderList (page, order => order.IsActive == true);
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null)
    {
        return GetOrderList (page, order => order.IsActive == false);
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id) 
    {
        var order = dataContext.ProdOrders
            .Include(order => order.Fabric.FabricType)
            .Include(order => order.Fabric.Color)
            .Include(order => order.Decor.Color)
            .Include(order => order.Customer)
            .Include(order => order.Notifications)
            .Include(order => order.Design)
            .Where(order => order.Id == id && order.CustomerId == user.ConfirmedId)
            .FirstOrDefault();

        if (order == null) {
            return NotFound (new BadFieldResult("id"));
        }

        return Ok (new ProdOrderResult(order).ForCustomer());
    }

    [HttpPost("create")]
    public IActionResult Create (ProdOrderRequest request)
    {
        var order = new ProdOrder() {
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
    public IActionResult Update (int id, ProdOrderRequest request)
    {
        var order = dataContext.ProdOrders
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

    protected IActionResult OnApplyRequest (ProdOrder order, ProdOrderRequest request)
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

        var fabric = dataContext.Resources
            .FirstOrDefault(f => f.SlotName == SlotNames.Fabric && f.Id == request.FabricId);

        if (fabric == null) {
            return BadRequest (new BadFieldResult("fabricId"));
        }

        var decor = dataContext.Resources
            .FirstOrDefault(m => m.SlotName == SlotNames.Decor && m.Id == request.DecorId);

        if (decor == null && design.DecorUsage > 0) {
            return BadRequest (new BadFieldResult("decorMaterialId"));
        }

        var customer = dataContext.Customers
            .Include(customer => customer.ProdOrders)
            .FirstOrDefault(customer => customer.Id == user.ConfirmedId);

        customer.UpdateDiscountPercent();

        int fixedDesignPrice = (int)(design.Price 
            + fabric.PricePerUnit * design.FabricUsage
            + (decor?.PricePerUnit ?? 0) * design.DecorUsage);

        request.ApplyTo(order);

        order.DecorId = decor?.Id;
        order.PricePerOne = fixedDesignPrice.WithDiscountFor(customer);

        return null; // null means nothing gone wrong
    }

    [HttpPost("confirm-submit/{id}")]
    public IActionResult ConfirmSubmit (int id)
    {
        var order = dataContext.ProdOrders
            .FirstOrDefault(order => order.Id == id && order.CustomerId == user.ConfirmedId);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        bool canSubmit = order.Status == OrderStatus.Unknown;

        if (! canSubmit) {
            return BadRequest ( new FailResult (
                $"Can not confirm submission of Production Order {id} with status {order.Status}"
            ));
        }

        try {
            order.Status = OrderStatus.Submitted;
            dataContext.ProdOrders.Update(order);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult (
                $"Successfully confirmed submission of Production Order {id}"
            ));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError ( new FailResult (
                $"Something went wrong while trying to confirm submission of Production Order {id}"
            ));
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete (int id)
    {
        var order = dataContext.ProdOrders
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
        IQueryable<ProdNotification> notifications = dataContext.ProdOrders
            .Include(n => n.Notifications)
            .Where(order => order.CustomerId == user.ConfirmedId)
            .SelectMany(order => order.Notifications)
            .Where(n => n.IsRead == false);

        if (order is int orderId) {
            notifications = notifications.Where(n => n.ProdOrderId == orderId);
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
            dataContext.ProdNotifications.UpdateRange(selectedNotifications);
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
