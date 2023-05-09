namespace Fwsh.WebApi.Controllers.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
[Route("manager/orders/production")]
public class ProdOrderController : FwshController
{
    const int PAGESIZE = 10;

    public ProdOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected IActionResult GetOrderList (int? page, int? design, Expression<Func<ProdOrder, bool>> condition)
    {
        if (page == null) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<ProdOrder> orders = dataContext.ProdOrders
            .Include(order => order.Design)
            .Where(condition);

        if (design is int designId) {
            orders = orders.Where(order => order.DesignId == designId);
        }

        return Ok ( orders.OrderBy(order => order.Id).Paginate (
            (int)page, PAGESIZE, order => new ProdOrderResult(order).Mini()
        ));
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, int? design = null)
    {
        return GetOrderList (page, design, order => order.IsActive);
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? design = null)
    {
        return GetOrderList (page, design, order => order.IsActive == false);
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
            .FirstOrDefault(order => order.Id == id);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new ProdOrderResult(order).ForManager() );
    }

    [HttpPost("set-status/{id}")]
    public IActionResult SetStatus (int id, [FromBody] string status)
    {
        if (! OrderStatus.Contains(status)) 
            return BadRequest(new BadFieldResult("status"));

        var order = dataContext.ProdOrders
            .FirstOrDefault(order => order.Id == id);

        if (order == null) 
            return NotFound(new BadFieldResult("id"));

        try {
            order.TrySetStatus(status);
            dataContext.ProdOrders.Update(order);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult($"Successfully set status '{status}' for ProdOrder {id}") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong"));
        }
    }

    [HttpPost("notify/{id}")]
    public IActionResult Notify (int id, [FromBody] string notificationText)
    {
        var notification = new ProdNotification() {
            ProdOrderId = id,
            Text = notificationText
        };

        try {
            dataContext.ProdNotifications.Add(notification);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult($"Successfully notified ProdOrder {id}") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError ( new FailResult($"Something went wrong while trying to notify ProdOrder {id}") );
        }
    }

    [HttpGet("preview-tasks/{id}")]
    public IActionResult PreviewTasks (int id)
    {
        var order = dataContext.ProdOrders
            .Include(order => order.Design.Tasks)
            .FirstOrDefault(order => order.Id == id);

        if (order == null) {
            return BadRequest(new BadFieldResult("id"));
        }

        if (order.Status != OrderStatus.Submitted) {
            return BadRequest(new FailResult($"Can not create tasks for Production Order {id}"));
        } 

        var tasks = Enumerable.Range(0, order.Quantity)
            .SelectMany (_ => order.Design.Tasks
                .Select (tp => new ProdTask() {
                    Status = TaskStatus.Unknown,
                    FurnitureId = 0,
                    PrototypeId = tp.Id,
                    WorkerId = null,
                    Prototype = tp
                })
            )
            .ToArray();

        // tasks[0].WorkerId = 1;
        // tasks[0].Status = TaskStatus.Assigned;
        // tasks[5].WorkerId = 1;
        // tasks[5].Status = TaskStatus.Working;

        var groupedTasks = tasks
            .GroupBy(t => new { PrototypeId = t.PrototypeId, WorkerId = t.WorkerId })
            .Select(g => new MultiProdTaskResult(g).Mini());

        return Ok(groupedTasks);
    }
}
