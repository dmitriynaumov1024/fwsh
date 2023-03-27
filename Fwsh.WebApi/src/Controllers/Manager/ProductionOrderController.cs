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
public class ProductionOrderController : FwshController
{
    const int PAGESIZE = 10;

    public ProductionOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected IActionResult GetOrderList (int? page, int? design, Expression<Func<ProductionOrder, bool>> condition)
    {
        if (page == null) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<ProductionOrder> orders = dataContext.ProductionOrders
            .Include(order => order.Design)
            .Where(condition);

        if (design is int designId) {
            orders = orders.Where(order => order.DesignId == designId);
        }

        return Ok ( orders.OrderBy(order => order.Id).Paginate (
            (int)page, PAGESIZE, order => new ProductionOrderResult(order).Mini()
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
        var order = dataContext.ProductionOrders
            .Include(order => order.Fabric.FabricType)
            .Include(order => order.Fabric.Color)
            .Include(order => order.DecorMaterial.Color)
            .Include(order => order.Customer)
            .Include(order => order.Notifications)
            .Include(order => order.Design)
            .ThenInclude(design => design.Photos)
            .FirstOrDefault(order => order.Id == id);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new ProductionOrderResult(order).ForManager() );
    }

    [HttpPost("set-status/{id}")]
    public IActionResult SetStatus (int id, [FromBody] string status)
    {
        if (! OrderStatus.Contains(status)) {
            return BadRequest(new BadFieldResult("status"));
        }

        var order = dataContext.ProductionOrders
            .FirstOrDefault(order => order.Id == id);

        if (order == null) {
            return NotFound(new BadFieldResult("id"));
        }

        try {
            order.Status = status;
            dataContext.ProductionOrders.Update(order);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult($"Successfully set status '{status}' for ProductionOrder {id}") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong"));
        }
    }

    [HttpPost("notify/{id}")]
    public IActionResult Notify (int id, [FromBody] string notificationText)
    {
        var notification = new ProductionNotification() {
            ProductionOrderId = id,
            Text = notificationText
        };

        try {
            dataContext.ProductionNotifications.Add(notification);
            dataContext.SaveChanges();
            return Ok ( new SuccessResult($"Successfully notified ProductionOrder {id}") );
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError ( new FailResult($"Something went wrong while trying to notify ProductionOrder {id}") );
        }
    }

    [HttpPost("create-tasks/{id}")]
    public IActionResult CreateTasks (int id)
    {
        var order = dataContext.ProductionOrders
            .Include(order => order.Design.TaskPrototypes)
            .FirstOrDefault(order => order.Id == id);

        if (order == null) {
            return BadRequest(new BadFieldResult("id"));
        }

        if (order.Status != OrderStatus.Submitted) {
            return BadRequest(new FailResult($"Can not create tasks for Production Order {id}"));
        } 

        var tasks = Enumerable.Range(0, order.Quantity)
            .SelectMany (_ => order.Design.TaskPrototypes
                .Select (tp => new ProductionTask() {
                    Status = TaskStatus.Unknown,
                    OrderId = order.Id,
                    PrototypeId = tp.Id,
                    WorkerId = null
                })
            );

        try {
            dataContext.ProductionTasks.AddRange(tasks);
            order.Status = OrderStatus.Delayed;
            dataContext.ProductionOrders.Update(order);
            dataContext.SaveChanges();
            return Ok ( new CreationResult (
                tasks.Select(t => t.Id).ToList(), 
                $"Successfully created Production Tasks for Order {id}"
            ));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to create Production Tasks"));
        }
    }

    [HttpGet("preview-tasks/{id}")]
    public IActionResult PreviewTasks (int id)
    {
        var order = dataContext.ProductionOrders
            .Include(order => order.Design.TaskPrototypes)
            .FirstOrDefault(order => order.Id == id);

        if (order == null) {
            return BadRequest(new BadFieldResult("id"));
        }

        if (order.Status != OrderStatus.Submitted) {
            return BadRequest(new FailResult($"Can not create tasks for Production Order {id}"));
        } 

        var tasks = Enumerable.Range(0, order.Quantity)
            .SelectMany (_ => order.Design.TaskPrototypes
                .Select (tp => new ProductionTask() {
                    Status = TaskStatus.Unknown,
                    OrderId = order.Id,
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
            .Select(g => new MultiProductionTaskResult(g).Mini());

        return Ok(groupedTasks);
    }
}
