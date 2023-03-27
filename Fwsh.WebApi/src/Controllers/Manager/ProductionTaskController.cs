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
    public IActionResult List (int? page = null, int? design = null, int? order = null)
    {
        IQueryable<ProductionTask> tasks = dataContext.ProductionTasks
            .Include(t => t.Order.Design)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .Where(t => t.Status == TaskStatus.Unknown
                     || t.Status == TaskStatus.Rejected
                     || t.Status == TaskStatus.Assigned 
                     || t.Status == TaskStatus.Working);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Order.DesignId == designId);
        }

        if (order is int orderId) {
            tasks = tasks.Where(t => t.OrderId == orderId);
        }

        var groupedTasks = tasks.ToList()
            .GroupBy(t => new { OrderId = t.OrderId, PrototypeId = t.PrototypeId, WorkerId = t.WorkerId })
            .Select(g => new MultiProductionTaskResult(g).Mini());

        return Ok(new ListResult<MultiProductionTaskResult>(groupedTasks));
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? design = null, int? order = null)
    {
        IQueryable<ProductionTask> tasks = dataContext.ProductionTasks
            .Include(t => t.Order.Design)
            .Include(t => t.Prototype)
            .Include(t => t.Worker)
            .Where(t => t.Status == TaskStatus.Finished
                     || t.Status == TaskStatus.Impossible);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Order.DesignId == designId);
        }

        if (order is int orderId) {
            tasks = tasks.Where(t => t.OrderId == orderId);
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

}
