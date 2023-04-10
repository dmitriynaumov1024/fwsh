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
using Fwsh.WebApi.FileStorage;
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
    const int MAX_PHOTOS = 8;
    const int FILE_SIZE_LIMIT = (1 << 21); // 2 Megabytes

    protected FileStorageProvider storage;

    public RepairOrderController (FwshDataContext dataContext, Logger logger, FwshUser user, FileStorageProvider storage)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
        this.storage = storage;
    }

    protected IActionResult GetOrderList (int? page, Expression<Func<RepairOrder, bool>> condition)
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
    public IActionResult Create (RepairOrderRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        var order = new RepairOrder() {
            CustomerId = user.ConfirmedId
        };

        try {
            request.ApplyTo(order);
            dataContext.RepairOrders.Add(order);
            dataContext.SaveChanges();
            return Ok ( new CreationResult(order.Id, "Successfully created Repair Order") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError ( new FailResult("Something went wrong") );
        }
    } 

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, RepairOrderRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        var order = dataContext.RepairOrders
            .Include(order => order.Photos)
            .FirstOrDefault(order => order.Id == id && order.CustomerId == user.ConfirmedId);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        bool canUpdate = order.Status == OrderStatus.Unknown;

        if (! canUpdate) {
            return BadRequest(new FailResult($"Can not update Repair Order {id} because of its status '{order.Status}'"));
        }

        try {
            request.ApplyTo(order);
            dataContext.RepairOrders.Update(order);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Successfully updated Repair Order {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to update Repair Order"));
        }
    }

    [HttpPost("confirm-submit/{id}")]
    public IActionResult ConfirmSubmit (int id)
    {
        var order = dataContext.RepairOrders
            .FirstOrDefault(order => order.Id == id && order.CustomerId == user.ConfirmedId);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        if (order.Status == OrderStatus.Unknown) {
            order.Status = OrderStatus.Submitted;
            dataContext.RepairOrders.Update(order);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult (
                $"Successfully confirmed submission of Repair Order {id}"
            ));
        }
        else {
            return BadRequest ( new FailResult (
                $"Can not confirm submission of Repair Order {id} with status {order.Status}"
            ));
        }
    }

    [HttpPost("attach-photos/{orderId}")]
    public IActionResult AttachPhotos (int orderId)
    {
        var order = dataContext.RepairOrders
            .Include(order => order.Photos)
            .Where(order => order.Id == orderId && order.CustomerId == user.ConfirmedId)
            .FirstOrDefault();

        if (order == null) {
            return NotFound (new BadFieldResult("orderId"));
        }

        var requestPhotos = this.Request.Form.Files.ToList();

        int count = 0, pos = order.Photos.Max(p => p.Position) + 1;
        foreach (var photo in requestPhotos) {
            if (order.Photos.Count >= MAX_PHOTOS) break;
            string ext = photo.FileName.Split('.').LastOrDefault();
            string url = $"repair-order-{order.Id}-{pos}-{Guid.NewGuid()}.{ext}"; 
            if (storage.TrySave(photo.OpenReadStream(), url)) {
                order.Photos.Add(new RepairOrderPhoto { 
                    Url = url, 
                    Position = pos 
                });
                count += 1;
                pos += 1;
            }
        }

        try {
            dataContext.RepairOrders.Update(order);
            dataContext.SaveChanges();
            return Ok(new SuccessResult($"Successfully attached {count} photos to Repair Order {orderId}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError("Something went wrong while trying to attach photos to Repair Order");
        }
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
            return Ok (new DeletionResult(order.Id, "Successfully deleted Repair Order"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong"));
        }
    }

    [HttpPost("read-notifications")]
    public IActionResult ReadNotifications (int? order = null, int? id = null, int? last = null)
    {
        IQueryable<RepairNotification> notifications = dataContext.RepairOrders
            .Include(n => n.Notifications)
            .Where(order => order.CustomerId == user.ConfirmedId)
            .SelectMany(order => order.Notifications)
            .Where(n => n.IsRead == false);

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
