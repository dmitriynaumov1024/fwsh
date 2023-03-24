namespace Fwsh.WebApi.Controllers.Manager;

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
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/orders/repair")]
public class RepairOrderController : FwshController
{
    const int PAGESIZE = 10;

    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public RepairOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected IActionResult GetOrderList (int? page, Expression<Func<RepairOrder, bool>> condition)
    {
        if (page == null) {
            return BadRequest (new BadFieldResult("page"));
        }

        IQueryable<RepairOrder> orders = dataContext.RepairOrders
            .Include(order => order.Photos)
            .Where(condition);

        return Ok ( orders.OrderBy(order => order.Id).Paginate (
            (int)page, PAGESIZE, order => new RepairOrderResult(order).Mini()
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
        var order = dataContext.RepairOrders
            .Include(order => order.Customer)
            .Include(order => order.Notifications)
            .Include(order => order.Photos)
            .Where(order => order.Id == id)
            .FirstOrDefault();

        if (order == null) {
            return NotFound (new BadFieldResult("id"));
        }

        return Ok ( new RepairOrderResult(order).ForManager() );
    }

    [HttpPost("set-status/{id}")]
    public IActionResult SetStatus (int id, [FromBody] string status)
    {
        if (! OrderStatus.Contains(status)) {
            return BadRequest(new BadFieldResult("status"));
        }

        var order = dataContext.RepairOrders
            .Where(order => order.Id == id)
            .FirstOrDefault();

        if (order == null) {
            return NotFound (new BadFieldResult("id"));
        }

        try {
            order.Status = status;
            dataContext.RepairOrders.Update(order);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult($"Successfully set status '{status}' for RepairOrder {id}") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong"));
        }
    }

    [HttpPost("notify/{id}")]
    public IActionResult Notify (int id, [FromBody] string notificationText)
    {
        var notification = new RepairNotification() {
            RepairOrderId = id,
            Text = notificationText
        };

        try {
            dataContext.RepairNotifications.Add(notification);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult($"Successfully notified RepairOrder {id}") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError ( new FailResult($"Something went wrong while trying to notify RepairOrder {id}") );
        }
    }
}
