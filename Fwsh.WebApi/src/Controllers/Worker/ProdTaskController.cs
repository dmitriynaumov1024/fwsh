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
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("worker/tasks/production")]
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
    public IActionResult List (int? page = null, int? design = null)
    {
        IQueryable<ProdTask> tasks = dataContext.ProdTasks
            .Include(t => t.Furniture.Design)
            .Include(t => t.Furniture.Order)
            .Include(t => t.Prototype)
            .Where(t => t.WorkerId == user.ConfirmedId)
            .Where(t => t.Furniture.Order.IsActive);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Furniture.DesignId == designId);
        }

        var groupedTasks = tasks.ToList()
            .GroupBy(t => t.PrototypeId)
            .Select(g => new MultiProdTaskResult(g).Mini());

        return Ok(new ListResult<MultiProdTaskResult>(groupedTasks));
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? design = null)
    {
        IQueryable<ProdTask> tasks = dataContext.ProdTasks
            .Include(t => t.Furniture.Design)
            .Include(t => t.Furniture.Order)
            .Include(t => t.Prototype)
            .Where(t => t.WorkerId == user.ConfirmedId)
            .Where(t => t.Furniture.Order.IsActive == false);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Furniture.DesignId == designId);
        }

        if (page is int pagenumber) {
            return Ok ( tasks.OrderBy(task => task.Id)
                .Paginate ( pagenumber, PAGESIZE, 
                    task => new ProdTaskResult(task).Mini()
                ) 
            );
        }
        else {
            return BadRequest (new BadFieldResult("page"));
        }
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        var task = dataContext.ProdTasks
            .Include(t => t.Furniture.Design)
            .Include(t => t.Furniture.Fabric)
            .Include(t => t.Furniture.Decor)
            .Include(t => t.Furniture.Order.Customer)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .FirstOrDefault(t => t.Id == id && t.WorkerId == user.ConfirmedId);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        return Ok(new ProdTaskResult(task).ForWorker());
    }

    [HttpPost("set-status/{id}")]
    public IActionResult SetStatus (int id, [FromBody] string status)
    {
        var task = dataContext.ProdTasks
            .Include(task => task.Furniture.Order.Furnitures)
            .ThenInclude(order => order.Tasks)
            .FirstOrDefault(task => task.Id == id && task.WorkerId == user.ConfirmedId);

        if (task == null) {
            return NotFound(new BadFieldResult("id")); 
        }

        if (! TaskStatus.Contains(status)) {
            return BadRequest(new BadFieldResult("status"));
        }

        var order = task.Furniture.Order;

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
                && order.Furnitures.All(f => f.Tasks.All(t => t.Status == TaskStatus.Finished))) {
                order.TrySetStatus(OrderStatus.Finished);
            }
            if (status == TaskStatus.Working) {
                task.StartedAt ??= DateTime.UtcNow;
                order.TrySetStatus(OrderStatus.Working);
            }
            dataContext.ProdTasks.Update(task);
            dataContext.ProdOrders.Update(task.Furniture.Order);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Successfully set status '{status}' for Production Task {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to set status for Production Task"));
        }

    }
}