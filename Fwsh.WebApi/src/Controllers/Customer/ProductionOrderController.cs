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
using Fwsh.WebApi.Requests.Customer;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("customer/orders/production")]
public class ProductionOrderController : ControllerBase
{
    const int PAGESIZE = 10;

    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

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
            order.Status == OrderStatus.Submitted 
            || order.Status == OrderStatus.Production 
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
            || order.Status == OrderStatus.Unknown
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
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new ProductionOrderResult(order).ForCustomer() );
    }

    [HttpPost("create")]
    public IActionResult Create (ProductionOrderCreationRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        var design = dataContext.Designs.Find(request.DesignId);

        if (design == null) {
            return BadRequest(new BadFieldResult("designId"));
        }

        var fabric = dataContext.Fabrics.Find(request.FabricId);

        if (fabric == null) {
            return BadRequest(new BadFieldResult("fabricId"));
        }

        var decorMaterial = dataContext.Materials.Find(request.DecorMaterialId);

        if (decorMaterial != null && ! decorMaterial.IsDecorative) {
            return BadRequest(new BadFieldResult("decorMaterialId"));
        }

        var customer = dataContext.Customers
            .Include(customer => customer.ProductionOrders)
            .Where(customer => customer.Id == user.ConfirmedId)
            .FirstOrDefault();

        customer.UpdateDiscountPercent();

        int fixedDesignPrice = (int)(design.Price 
            + fabric.PricePerUnit * design.FabricQuantity 
            + (decorMaterial?.PricePerUnit ?? 0) * design.DecorMaterialQuantity);

        var order = new ProductionOrder() {
            Status = OrderStatus.Submitted,
            CustomerId = user.ConfirmedId,
            Quantity = request.Quantity,
            PricePerOne = fixedDesignPrice.WithDiscountFor(customer),
            DesignId = request.DesignId,
            FabricId = request.FabricId,
            DecorMaterialId = request.DecorMaterialId
        };

        try {
            dataContext.Add(order);
            dataContext.SaveChanges();
            return Ok(new CreationResult(order.Id, "Successfully created new Production Order"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return BadRequest(new FailResult("Something went wrong"));
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete (int id)
    {
        var order = dataContext.ProductionOrders
            .Where(order => order.Id == id && order.CustomerId == user.ConfirmedId)
            .FirstOrDefault();

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        bool canSafelyDelete = order.Status == OrderStatus.Submitted 
                            || order.Status == OrderStatus.Unknown;

        if (! canSafelyDelete) {
            return BadRequest(new FailResult("Order status does not allow deletion"));
        }

        try {
            dataContext.Remove(order);
            dataContext.SaveChanges();
            return Ok(new DeletionResult(order.Id, "Successfully deleted Production Order"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return BadRequest(new FailResult("Something went wrong"));
        }
    }

    [HttpGet("read-notifications")]
    public IActionResult ReadNotifications (int? order = null, int? last = null, bool all = false)
    {
        IQueryable<ProductionNotification> notifications = 
            dataContext.ProductionNotifications.Where(n => n.IsRead == false);

        if (order is int orderId) {
            notifications = notifications.Where(n => n.ProductionOrderId == orderId);
            if (last is int lastNotificationId) {
                notifications = notifications.Where(n => n.Id <= lastNotificationId);    
            }
            else if (! all) {
                return BadRequest(new BadFieldResult("all", "last"));
            }
        }
        else {
            return BadRequest(new BadFieldResult("order"));
        }
        
        try {
            var selectedNotifications = notifications.ToList();
            foreach (var n in selectedNotifications) n.IsRead = true;
            dataContext.ProductionNotifications.UpdateRange(selectedNotifications);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Production order {order}: Notifications until {last} were marked as read"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return BadRequest(new FailResult("Something went wrong"));
        }
    }
}
