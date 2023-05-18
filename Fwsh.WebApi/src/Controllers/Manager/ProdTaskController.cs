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
public class ProdTaskController : FwshController
{
    const int PAGESIZE = 20;

    public ProdTaskController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, int? design = null, int? order = null, int? worker = null)
    {
        IQueryable<ProdTask> tasks = dataContext.ProdTasks
            .Include(t => t.Furniture.Design)
            .Include(t => t.Furniture.Order)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .Where(t => t.Furniture.Order.IsActive);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Prototype.DesignId == designId);
        }

        if (order is int orderId) {
            tasks = tasks.Where(t => t.Furniture.OrderId == orderId);
        }

        if (worker is int workerId) {
            tasks = tasks.Where(t => t.WorkerId == workerId);
        }

        var groupedTasks = tasks.ToList()
            .GroupBy(t => new { OrderId = t.Furniture.OrderId, PrototypeId = t.PrototypeId, WorkerId = t.WorkerId })
            .Select(g => new MultiProdTaskResult(g).Mini());

        return Ok(new ListResult<MultiProdTaskResult>(groupedTasks));
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? design = null, int? order = null, int? worker = null)
    {
        IQueryable<ProdTask> tasks = dataContext.ProdTasks
            .Include(t => t.Furniture.Design)
            .Include(t => t.Furniture.Order)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .Where(t => t.Furniture.Order.IsActive == false);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Furniture.DesignId == designId);
        }

        if (order is int orderId) {
            tasks = tasks.Where(t => t.Furniture.OrderId == orderId);
        }

        if (worker is int workerId) {
            tasks = tasks.Where(t => t.WorkerId == workerId);
        }

        if (page is int pagenumber) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Paginate ( pagenumber, PAGESIZE, 
                    task => new ProdTaskResult(task).Mini()
                ) 
            );
        }
        else if (order != null) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Listiate ( Int32.MaxValue, 
                    task => new ProdTaskResult(task).Mini()
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

        IQueryable<ProdTask> tasks = dataContext.ProdTasks
            .Include(t => t.Furniture.Order.Customer)
            .Include(t => t.Furniture.Design)
            .Include(t => t.Worker)
            .Where(t => t.Furniture.Order.IsActive);

        if (groupby == "customer") return Ok (new {
            Key = groupby,
            Items = tasks.GroupBy(t => t.Furniture.Order.CustomerId)
                .Select(g => new { Key = g.First().Furniture.Order.Customer, Count = g.Count() }).ToList()
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
            Items = tasks.GroupBy(t => t.Furniture.Design.DisplayName)
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
        var task = dataContext.ProdTasks
            // .Include(t => t.Furniture.Order.Customer)
            .Include(t => t.Furniture.Design)
            .Include(t => t.Furniture.Decor)
            .Include(t => t.Furniture.Fabric)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .Include(t => t.Resources)
            .ThenInclude(r => r.Item.Stored)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        return Ok(new ProdTaskResult(task).ForManager());
    }

    [HttpGet("preview")]
    public IActionResult Preview (int? order = null, bool reuse = false)
    {
        if (order == null) {
            return BadRequest(new BadFieldResult("order"));
        }

        int orderId = (int)order;

        ProdOrder prodOrder = dataContext.ProdOrders
            .Include(order => order.Decor)
            .Include(order => order.Fabric)
            .Include(order => order.Design.Tasks)
            .ThenInclude(task => task.Resources)
            .FirstOrDefault(order => order.Id == orderId);

        if (prodOrder == null) {
            return BadRequest(new BadFieldResult("order"));
        }

        if (prodOrder.Status != OrderStatus.Submitted) {
            return BadRequest(new FailResult($"Can not create tasks for Production Order {orderId}"));
        } 

        int existingFurnitureCount = reuse ? 
            dataContext.ProdFurniture
                .Where(furn => furn.OrderId == null
                    && furn.DesignId == prodOrder.DesignId 
                    && furn.FabricId == prodOrder.FabricId 
                    && furn.DecorId == prodOrder.DecorId)
                .Take(prodOrder.Quantity).Count() : 0;


        return Ok ( new ListResult<MultiProdTaskResult> (
            prodOrder.Design.CreateFurniture(prodOrder).Tasks.Select(task => new MultiProdTaskResult {
                Quantity = prodOrder.Quantity - existingFurnitureCount,
                Prototype = new TaskPrototypeResult(task.Prototype).Mini()
            })
        ));
    }

    [HttpPost("create")]
    public IActionResult Create (int? order = null, bool reuse = false)
    {
        if (order == null) {
            return BadRequest(new BadFieldResult("order"));
        }

        int orderId = (int)order;

        ProdOrder prodOrder = dataContext.ProdOrders
            .Include(order => order.Decor)
            .Include(order => order.Fabric)
            .Include(order => order.Design.Tasks)
            .ThenInclude(task => task.Resources)
            .FirstOrDefault(order => order.Id == orderId);

        if (prodOrder == null) {
            return BadRequest(new BadFieldResult("order"));
        }

        if (prodOrder.Status != OrderStatus.Submitted) {
            return BadRequest(new FailResult($"Can not create tasks for Production Order {orderId}"));
        } 

        List<ProdFurniture> existingFurniture = reuse ? 
            dataContext.ProdFurniture
                .Where(furn => furn.OrderId == null
                    && furn.DesignId == prodOrder.DesignId 
                    && furn.FabricId == prodOrder.FabricId 
                    && furn.DecorId == prodOrder.DecorId)
                .Take(prodOrder.Quantity).ToList() :
            new List<ProdFurniture>();

        foreach (var furniture in existingFurniture) {
            furniture.OrderId = order;
            prodOrder.Furnitures.Add(furniture);
        }

        prodOrder.Furnitures.AddRange (
            Enumerable.Range(existingFurniture.Count, prodOrder.Quantity)
                .Select(_ => prodOrder.Design.CreateFurniture(prodOrder))
        );

        try {
            prodOrder.Status = OrderStatus.Delayed;
            dataContext.ProdOrders.Update(prodOrder);
            dataContext.SaveChanges();
            return Ok ( new CreationResult (
                dataContext.ProdTasks
                    .Include(t => t.Furniture)
                    .Where(t => t.Furniture.OrderId == orderId)
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
    public IActionResult SetStatus (int id, string status)
    {
        var task = dataContext.ProdTasks
            .Include(task => task.Furniture)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        if (! TaskStatus.Contains(status)) {
            return BadRequest(new BadFieldResult("status"));
        }

        bool canChangeStatus = task.Status == TaskStatus.Unknown
            || task.Furniture.Status == OrderStatus.Delayed
            || task.Furniture.Status == OrderStatus.Working
            || task.Furniture.Status == OrderStatus.Finished
                && (DateTime.UtcNow - (DateTime)task.Furniture.FinishedAt).Minutes < 10; 

        if (status != task.Status) try {
            task.TrySetStatus(status);
            dataContext.ProdTasks.Update(task);
            dataContext.SaveChanges();
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to set status for Production Task"));
        }

        return Ok (new SuccessResult($"Successfully set status '{status}' for Production Task {id}"));
    }

    [HttpPost("set-active/{id}")]
    public IActionResult SetActive (int id, bool active)
    {
        var task = dataContext.ProdTasks
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        if (active != task.IsActive) try {
            task.IsActive = active;
            dataContext.ProdTasks.Update(task);
            dataContext.SaveChanges();
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to set active"));
        }

        return Ok (new SuccessResult($"Successfully set active={active} for Repair Task {id}"));
    }

    [HttpPost("assign/{id}")]
    public IActionResult Assign (int id, int? worker = null)
    {
        if (worker == null || dataContext.Workers.FirstOrDefault(w => w.Id == worker) == null) {
            return BadRequest(new BadFieldResult("worker"));
        }

        var task = dataContext.ProdTasks
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
            dataContext.ProdTasks.Update(task);
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

        var tasks = dataContext.ProdTasks
            .Where(task => task.Furniture.OrderId == order 
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
            dataContext.ProdTasks.UpdateRange(tasks);
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
