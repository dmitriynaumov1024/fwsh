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
using Fwsh.WebApi.Requests.Manager;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/tasks/repair")]
public class RepairTaskController : FwshController
{
    const int PAGESIZE = 20;

    public RepairTaskController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, int? order = null, int? worker = null)
    {
        IQueryable<RepairTask> tasks = dataContext.RepairTasks
            .Include(t => t.Order)
            .Include(t => t.Worker)
            .Where(t => t.Order.IsActive);

        if (order is int orderId) {
            tasks = tasks.Where(t => t.OrderId == orderId);
        }

        if (worker is int workerId) {
            tasks = tasks.Where(t => t.WorkerId == workerId);
        }

        if (page is int pagenumber) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Paginate ( pagenumber, PAGESIZE, 
                    task => new RepairTaskResult(task).Mini()
                ) 
            );
        }
        else if (order != null || worker != null) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Listiate ( Int32.MaxValue, 
                    task => new RepairTaskResult(task).Mini()
                )
            );
        } 
        else {
            return BadRequest (new BadFieldResult("page"));
        }
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? order = null, int? worker = null)
    {
        IQueryable<RepairTask> tasks = dataContext.RepairTasks
            .Include(t => t.Order)
            .Include(t => t.Worker)
            .Where(t => t.Order.IsActive == false);

        if (order is int orderId) {
            tasks = tasks.Where(t => t.OrderId == orderId);
        }

        if (worker is int workerId) {
            tasks = tasks.Where(t => t.WorkerId == workerId);
        }

        if (page is int pagenumber) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Paginate ( pagenumber, PAGESIZE, 
                    task => new RepairTaskResult(task).Mini()
                ) 
            );
        }
        else if (order != null || worker != null) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Listiate ( Int32.MaxValue, 
                    task => new RepairTaskResult(task).Mini()
                )
            );
        } 
        else {
            return BadRequest (new BadFieldResult("page"));
        }
    }

    [HttpGet("count")]
    public IActionResult Count (string groupby = null, bool onlyactive = false)
    {
        if (groupby == null) {
            return BadRequest(new BadFieldResult("groupby"));
        }

        groupby = groupby.ToLower();

        IQueryable<RepairTask> tasks = dataContext.RepairTasks
            .Include(t => t.Order.Customer)
            .Include(t => t.Worker);

        if (onlyactive) tasks = tasks.Where ( t => 
               t.Order.Status == OrderStatus.Submitted 
            || t.Order.Status == OrderStatus.Working 
            || t.Order.Status == OrderStatus.Delayed 
            || t.Order.Status == OrderStatus.Finished );

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
        var task = dataContext.RepairTasks
            .Include(t => t.Order.Customer)
            .Include(t => t.Worker)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        return Ok(new RepairTaskResult(task).ForManager());
    }

    [HttpPost("create")]
    public IActionResult Create (RepairTaskRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? 
                "Something went wrong while validating creation request"));
        }

        var order = dataContext.RepairOrders
            .FirstOrDefault(order => order.Id == request.OrderId);

        if (order == null) {
            return BadRequest(new BadFieldResult("orderId"));
        }

        bool canCreate = order.Status == OrderStatus.Unknown
                      || order.Status == OrderStatus.Submitted
                      || order.Status == OrderStatus.Delayed
                      || order.Status == OrderStatus.Working;

        if (! canCreate) {
            return BadRequest(new FailResult($"Can not create tasks for Repair Order {order.Id}"));
        }

        RepairTask task = request.Create();

        try {
            dataContext.RepairTasks.Add(task);
            dataContext.SaveChanges();
            return Ok ( new CreationResult (task.Id, "Successfully created Repair Task"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to create Repair Task"));
        }
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, RepairTaskRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? 
                "Something went wrong while validating creation request"));
        }

        var task = dataContext.RepairTasks
            .FirstOrDefault(task => task.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id"));
        }

        bool canUpdate = task.Status == null 
                      || task.Status == TaskStatus.Unknown
                      || task.Status == TaskStatus.Assigned
                      || task.Status == TaskStatus.Rejected;

        if (! canUpdate) {
            return BadRequest(new FailResult("Can not update Repair Task"));
        }

        request.ApplyTo(task);

        try {
            dataContext.RepairTasks.Update(task);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully updated Repair Task"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to create Repair Task"));
        }
    }

    [HttpPost("set-status/{id}")]
    public IActionResult SetStatus (int id, string status)
    {
        var task = dataContext.RepairTasks
            .Include(task => task.Order)
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
            dataContext.RepairTasks.Update(task);
            dataContext.SaveChanges();
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to set status for Repair Task"));
        }

        return Ok (new SuccessResult($"Successfully set status '{status}' for Repair Task {id}"));
    }

    [HttpPost("set-active/{id}")]
    public IActionResult SetActive (int id, bool active)
    {
        var task = dataContext.RepairTasks
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        if (active != task.IsActive) try {
            task.IsActive = active;
            dataContext.RepairTasks.Update(task);
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

        var task = dataContext.RepairTasks
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        bool canAssign = task.Status == TaskStatus.Unknown 
                        || task.Status == TaskStatus.Assigned 
                        || task.Status == TaskStatus.Rejected;

        if (! canAssign) {
            return BadRequest(new FailResult($"Can not assign Repair Task {id} because of status '{task.Status}'"));
        }

        try {
            task.WorkerId = worker;
            task.IsActive = true;
            task.Status = TaskStatus.Assigned;
            dataContext.RepairTasks.Update(task);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Successfully assigned Repair Task {id} to worker {worker}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to assign Repair Task"));
        }
    }

}
