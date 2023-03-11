namespace Fwsh.WebApi.Controllers.Customer;

using System;
using System.Collections.Generic;
using System.Linq;
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
using Fwsh.WebApi.Results.Customer;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("customer/orders/repair")]
public class RepairOrderController : ControllerBase
{
    const int FILE_SIZE_LIMIT = (1 << 21); // 2 Megabytes

    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public RepairOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected IQueryable<RepairOrder> GetOwnOrders()
    {
        return dataContext.RepairOrders
            .Include(order => order.Photos)
            .Where(order => order.CustomerId == user.ConfirmedId);
    }

    [HttpGet("list")]
    public IActionResult List () 
    {
        var orders = this.GetOwnOrders()
            .Where(order => order.Status == OrderStatus.Submitted 
                            || order.Status == OrderStatus.Production 
                            || order.Status == OrderStatus.Delayed 
                            || order.Status == OrderStatus.Finished );

        return Ok ( orders.Listiate (
            Int32.MaxValue, order => new MiniRepairOrderResult(order)
        ));
    }

    [HttpGet("archive")]
    public IActionResult Archive ()
    {
        var orders = this.GetOwnOrders()
            .Where(order => order.Status == OrderStatus.ReceivedAndPaid
                            || order.Status == OrderStatus.Rejected 
                            || order.Status == OrderStatus.Impossible
                            || order.Status == OrderStatus.Unknown );

        return Ok ( orders.Listiate (
            Int32.MaxValue, order => new MiniRepairOrderResult(order)
        ));
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

        return Ok ( new RepairOrderResult(order) );
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
            logger.Error("{0}", ex);
            return BadRequest ( new MessageResult("Something went wrong") );
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
            return BadRequest(new MessageResult("Something went wrong"));
        }
    }
}
