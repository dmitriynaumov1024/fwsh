namespace Fwsh.WebApi.Controllers.Worker;

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
[Route("worker/tasks/repair")]
public class RepairTaskController : FwshController
{
    const int PAGESIZE = 20;

    public RepairTaskController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    IActionResult GetTaskList (int? page, Expression<Func<RepairTask, bool>> condition)
    {
        if (page == null) 
            return BadRequest(new BadFieldResult("page"));

        IQueryable<RepairTask> tasks = dataContext.RepairTasks
            .Include(t => t.Order)
            .Where(t => t.WorkerId == user.ConfirmedId)
            .Where(condition);

        return Ok ( tasks.OrderBy(t => t.Id).Paginate ( 
            (int)page, PAGESIZE, task => new RepairTaskResult(task).Mini()
        ));
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null)
    {
        return GetTaskList (page, task => task.IsActive);
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null)
    {
        return GetTaskList (page, task => !task.IsActive);
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        var task = dataContext.RepairTasks
            .Include(t => t.Resources)
            .ThenInclude(r => r.Item.Stored)
            .FirstOrDefault(t => t.Id == id && t.WorkerId == user.ConfirmedId);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        return Ok(new RepairTaskResult(task).ForWorker());
    }

    [HttpPost("set-status/{id}")]
    public IActionResult SetStatus (int id, string status)
    {
        var task = dataContext.RepairTasks
            .Include(task => task.Order)
            .ThenInclude(order => order.Tasks)
            .FirstOrDefault(task => task.Id == id && task.WorkerId == user.ConfirmedId);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        if (! TaskStatus.Contains(status)) {
            return BadRequest(new BadFieldResult("status"));
        }

        var order = task.Order;

        bool canChangeStatus = 
            task.Status == TaskStatus.Assigned 
                && (status == TaskStatus.Working || status == TaskStatus.Rejected)
            || task.Status == TaskStatus.Rejected
                && (status == TaskStatus.Assigned || status == TaskStatus.Working)
            || task.Status == TaskStatus.Working 
                && (status == TaskStatus.Finished || status == TaskStatus.Rejected)
            || task.Status == TaskStatus.Finished 
                && (status == TaskStatus.Working && order.Status == OrderStatus.Working);

        if (! canChangeStatus) {
            return BadRequest(new BadFieldResult("status") {
                Message = $"Can not change status '{task.Status}' => '{status}'"
            });
        }

        try {
            task.TrySetStatus(status);
            if (task.Status == TaskStatus.Finished 
                && order.Tasks.All(t => t.Status == TaskStatus.Finished)) {
                order.TrySetStatus(OrderStatus.Finished);
            }
            if (status == TaskStatus.Working) {
                task.StartedAt ??= DateTime.UtcNow;
                order.TrySetStatus(OrderStatus.Working);
            }
            dataContext.RepairTasks.Update(task);
            dataContext.RepairOrders.Update(task.Order);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Successfully set status '{status}' for Repair Task {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to set status for Repair Task"));
        }

    }

    [HttpPost("set-usage/{taskId}")]
    public IActionResult SetUsage (int taskId, List<ResourceQuantity> resources)
    {
        RepairTask task = dataContext.RepairTasks
            .Include(t => t.Resources)
            .ThenInclude(r => r.Item.Stored)
            .FirstOrDefault(t => t.IsActive 
                && (t.Status == TaskStatus.Working || t.Status == TaskStatus.Finished)
                && t.WorkerId == user.ConfirmedId && t.Id == taskId);

        if (task == null) {
            return NotFound(new BadFieldResult("taskId"));
        }

        foreach (var res in resources) {
            var existingRes = task.Resources
                .FirstOrDefault(r => r.ItemId == res.ItemId);
            if (existingRes?.Item?.Stored == null) continue;
            double diff = Math.Min(res.ActualQuantity - existingRes.ActualQuantity, existingRes.Item.Stored.InStock);
            diff = Math.Max(diff, -existingRes.ActualQuantity);
            existingRes.ActualQuantity += diff;
            existingRes.Item.Stored.InStock -= diff;
        }

        try {
            dataContext.RepairTasks.Update(task);
            dataContext.SaveChanges();
            return Ok(new SuccessResult("Successfully set resource usage"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not set resource usage"));
        }
    }
}
