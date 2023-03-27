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
[Route("manager/tasks/production")]
public class ProductionTaskController : FwshController
{
    const int PAGESIZE = 20;

    public ProductionTaskController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, int? design = null, int? order = null, int? worker = null)
    {
        IQueryable<ProductionTask> tasks = dataContext.ProductionTasks
            .Include(t => t.Order.Design)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .Where(t => t.Order.Status == OrderStatus.Submitted 
                     || t.Order.Status == OrderStatus.Working 
                     || t.Order.Status == OrderStatus.Delayed );

        if (design is int designId) {
            tasks = tasks.Where(t => t.Order.DesignId == designId);
        }

        if (order is int orderId) {
            tasks = tasks.Where(t => t.OrderId == orderId);
        }

        if (worker is int workerId) {
            tasks = tasks.Where(t => t.WorkerId == workerId);
        }

        var groupedTasks = tasks.ToList()
            .GroupBy(t => new { OrderId = t.OrderId, PrototypeId = t.PrototypeId, WorkerId = t.WorkerId })
            .Select(g => new MultiProductionTaskResult(g).Mini());

        return Ok(new ListResult<MultiProductionTaskResult>(groupedTasks));
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? design = null, int? order = null, int? worker = null)
    {
        IQueryable<ProductionTask> tasks = dataContext.ProductionTasks
            .Include(t => t.Order.Design)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .Where(t => t.Order.Status == OrderStatus.ReceivedAndPaid
                     || t.Order.Status == OrderStatus.Rejected
                     || t.Order.Status == OrderStatus.Impossible
                     || t.Order.Status == OrderStatus.Finished );

        if (design is int designId) {
            tasks = tasks.Where(t => t.Order.DesignId == designId);
        }

        if (order is int orderId) {
            tasks = tasks.Where(t => t.OrderId == orderId);
        }

        if (worker is int workerId) {
            tasks = tasks.Where(t => t.WorkerId == workerId);
        }

        if (page is int pagenumber) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Paginate ( pagenumber, PAGESIZE, 
                    task => new ProductionTaskResult(task).Mini()
                ) 
            );
        }
        else if (order != null) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Listiate ( Int32.MaxValue, 
                    task => new ProductionTaskResult(task).Mini()
                )
            );
        } 
        else {
            return BadRequest (new BadFieldResult("page"));
        }
    }

    [HttpGet("count")]
    public IActionResult Count (string groupby = null)
    {
        if (groupby == null) {
            return BadRequest(new BadFieldResult("groupby"));
        }

        groupby = groupby.ToLower();

        IQueryable<ProductionTask> tasks = dataContext.ProductionTasks
            .Include(t => t.Order.Customer)
            .Include(t => t.Order.Design)
            .Include(t => t.Worker)
            .Where(t => t.Order.Status == OrderStatus.Submitted 
                     || t.Order.Status == OrderStatus.Working 
                     || t.Order.Status == OrderStatus.Delayed );

        if (groupby == "customer") return Ok (new {
            Key = groupby,
            Items = tasks.GroupBy(t => t.Order.CustomerId)
                .Select(g => new { Key = g.First().Order.Customer, Count = g.Count() }).ToList()
                .Select(g => new { Key = new CustomerResult(g.Key), Count = g.Count })
        });

        if (groupby == "worker") return Ok (new {
            Key = groupby,
            Items = tasks.GroupBy(t => t.WorkerId)
                .Select(g => new { Key = g.First().Worker, Count = g.Count() }).ToList()
                .Select(g => new { Key = (g.Key == null) ? null : new WorkerResult(g.Key), Count = g.Count })
        });

        if (groupby == "design") return Ok (new {
            Key = groupby,
            Items = tasks.GroupBy(t => t.Order.Design.DisplayName)
                .Select(g => new { Key = g.Key, Count = g.Count() })
        });

        if (groupby == "status") return Ok (new {
            Key = groupby, 
            Items = tasks.GroupBy(t => t.Status)
                .Select(g => new { Key = g.Key, Count = g.Count() })
        });

        return BadRequest(new BadFieldResult("groupby"));
    } 

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        var task = dataContext.ProductionTasks
            .Include(t => t.Order.Design)
            .Include(t => t.Order.Fabric)
            .Include(t => t.Order.DecorMaterial)
            .Include(t => t.Order.Customer)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        return Ok(new ProductionTaskResult(task).ForManager());
    }

    [HttpPost("create")]
    public IActionResult Create (int? order = null)
    {
        if (order == null) {
            return BadRequest(new BadFieldResult("order"));
        }

        int orderId = (int)order;

        var prodOrder = dataContext.ProductionOrders
            .Include(order => order.Design.TaskPrototypes)
            .FirstOrDefault(order => order.Id == orderId);

        if (prodOrder == null) {
            return BadRequest(new BadFieldResult("order"));
        }

        if (prodOrder.Status != OrderStatus.Submitted) {
            return BadRequest(new FailResult($"Can not create tasks for Production Order {orderId}"));
        } 

        var tasks = Enumerable.Range(0, prodOrder.Quantity)
            .SelectMany (_ => prodOrder.Design.TaskPrototypes
                .Select (tp => new ProductionTask() {
                    Status = TaskStatus.Unknown,
                    OrderId = prodOrder.Id,
                    PrototypeId = tp.Id,
                    WorkerId = null
                })
            );

        try {
            dataContext.ProductionTasks.AddRange(tasks);
            prodOrder.Status = OrderStatus.Delayed;
            dataContext.ProductionOrders.Update(prodOrder);
            dataContext.SaveChanges();
            return Ok ( new CreationResult (
                dataContext.ProductionTasks
                    .Where(t => t.OrderId == orderId)
                    .Select(t => t.Id).ToList(), 
                $"Successfully created Production Tasks for Order {orderId}"
            ));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to create Production Tasks"));
        }
    }

    [HttpPost("set-status/{id}")]
    public IActionResult SetStatus (int id, [FromBody] string status)
    {
        var task = dataContext.ProductionTasks
            .Include(task => task.Order.Tasks)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        if (! TaskStatus.Contains(status)) {
            return BadRequest(new BadFieldResult("status"));
        }

        bool canChangeStatus = 
               task.Order.Status == OrderStatus.Delayed
            || task.Order.Status == OrderStatus.Working
            || task.Order.Status == OrderStatus.Finished
                && (DateTime.UtcNow - (DateTime)task.Order.FinishedAt).Minutes < 10; 

        if (status != task.Status) try {
            task.TrySetStatus(status);
            dataContext.ProductionTasks.Update(task);
            dataContext.SaveChanges();
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to set status for Production Task"));
        }

        return Ok (new SuccessResult($"Successfully set status '{status}' for Production Task {id}"));
    }

    [HttpPost("assign/{id}")]
    public IActionResult Assign (int id, int? worker = null)
    {
        if (worker == null || dataContext.Workers.FirstOrDefault(w => w.Id == worker) == null) {
            return BadRequest(new BadFieldResult("worker"));
        }

        var task = dataContext.ProductionTasks
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        bool canAssign = task.Status == TaskStatus.Unknown 
                        || task.Status == TaskStatus.Assigned 
                        || task.Status == TaskStatus.Rejected;

        if (! canAssign) {
            return BadRequest(new FailResult($"Can not assign Production Task {id} because of status '{task.Status}'"));
        }

        try {
            task.WorkerId = worker;
            task.Status = TaskStatus.Assigned;
            dataContext.ProductionTasks.Update(task);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Successfully assigned Production Task {id} to worker {worker}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to assign Production Task"));
        }
    }

    [HttpPost("assign-many")]
    public IActionResult AssignMany (int? order = null, int? prototype = null, int? worker = null)
    {
        if (worker == null || dataContext.Workers.FirstOrDefault(w => w.Id == worker) == null) {
            return BadRequest(new BadFieldResult("worker"));
        }

        var tasks = dataContext.ProductionTasks
            .Where(task => task.OrderId == order 
                        && task.PrototypeId == prototype)
            .Where(task => task.Status == TaskStatus.Unknown 
                        || task.Status == TaskStatus.Assigned 
                        || task.Status == TaskStatus.Rejected)
            .ToList();

        foreach (var task in tasks) {
            task.WorkerId = worker;
            task.Status = TaskStatus.Assigned;
        }

        try {
            dataContext.ProductionTasks.UpdateRange(tasks);
            dataContext.SaveChanges();
            var ids = String.Join(", ", tasks.OrderBy(task => task.Id).Select(task => task.Id));
            return Ok (new SuccessResult($"Successfully assigned Production Tasks {ids} to Worker {worker}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to assign Production Tasks"));
        }
    }
}
