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
[Route("customer/orders/repair")]
public class RepairOrderController : FwshController
{
    const int PAGESIZE = 10;
    const int FILE_SIZE_LIMIT = (1 << 21); // 2 Megabytes

    public RepairOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected IActionResult GetOrderList (int? page, int? design, Expression<Func<RepairOrder, bool>> condition)
    {
        if (page == null) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<RepairOrder> orders = dataContext.RepairOrders
            .Include(order => order.Photos)
            .Where(order => order.CustomerId == user.ConfirmedId)
            .Where(condition);

        return Ok ( orders.OrderBy(order => order.Id).Paginate (
            (int)page, PAGESIZE, order => new RepairOrderResult(order).Mini()
        ));
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, int? design = null)
    {
        return GetOrderList (page, design, order => 
            order.Status == OrderStatus.Submitted 
            || order.Status == OrderStatus.Production 
            || order.Status == OrderStatus.Delayed 
            || order.Status == OrderStatus.Finished
        );
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? design = null)
    {
        return GetOrderList (page, design, order => 
            order.Status == OrderStatus.ReceivedAndPaid
            || order.Status == OrderStatus.Rejected 
            || order.Status == OrderStatus.Impossible
            || order.Status == OrderStatus.Unknown
        );
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        var order = dataContext.RepairOrders
            .Include(order => order.Customer)
            .Include(order => order.Photos)
            .Include(order => order.Notifications)
            .Where(order => order.Id == id && order.CustomerId == user.ConfirmedId)
            .FirstOrDefault();

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new RepairOrderResult(order).ForCustomer() );
    }

    [HttpPost("create")]
    public IActionResult Create (RepairOrderCreationRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        var order = new RepairOrder() {
            CustomerId = user.ConfirmedId,
            Description = request.Description
        };

        for (int i=0; i<request.PhotoCount; i++) {
            order.Photos.Add(new RepairOrderPhoto() {
                // don't care about order id, it shall be set automatically
                Position = i,
                Url = ""
            });
        }
        try {
            dataContext.RepairOrders.Add(order);
            dataContext.SaveChanges();
            return Ok ( new CreationResult(order.Id, "Successfully created Repair Order") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError ( new FailResult("Something went wrong") );
        }
    } 

    [HttpPost("attach-photos/{orderid}")]
    public IActionResult AttachPhotos (int orderid)
    {
        var order = dataContext.RepairOrders
            .Include(order => order.Photos)
            .Where(order => order.Id == orderid && order.CustomerId == user.ConfirmedId)
            .FirstOrDefault();

        if (order == null) {
            return NotFound (new BadFieldResult("orderid"));
        }

        var requestPhotos = this.Request.Form.Files.ToList(); 

        if (requestPhotos.Count != order.Photos.Count
            || requestPhotos.Any(photo => photo.Length >= FILE_SIZE_LIMIT)) {
            return BadRequest (new BadFieldResult("photos"));
        }

        var orderPhotos = order.Photos.OrderBy(p => p.Position);

        foreach (var (photo, orderPhoto) in requestPhotos.Zip(orderPhotos)) {
            // TODO implement later
            string ext = photo.FileName.Split('.').LastOrDefault();
            if (ext == null) {
                return BadRequest (new BadFieldResult("photos"));
            }
            orderPhoto.Url = $"repair-{order.Id}-{orderPhoto.Position}-{Guid.NewGuid()}.{ext}"; 
        }

        order.Status = OrderStatus.Submitted;

        dataContext.RepairOrders.Update(order);
        dataContext.SaveChanges();

        return Ok (new SuccessResult($"Successfully attached photos to {order.Id}"));
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete (int id = 0)
    {
        var order = dataContext.RepairOrders
            .Include(order => order.Photos)
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

    [HttpGet("read-notifications")]
    public IActionResult ReadNotifications (int? order = null, int? id = null, int? last = null)
    {
        IQueryable<RepairNotification> notifications = 
            dataContext.RepairNotifications.Where(n => n.IsRead == false);

        if (order is int orderId) {
            notifications = notifications.Where(n => n.RepairOrderId == orderId);
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
            dataContext.RepairNotifications.UpdateRange(selectedNotifications);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult ( 
                $"Repair order {order}: Notification " 
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
