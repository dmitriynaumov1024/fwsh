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
public class ProductionTaskController : FwshController
{
    public ProductionTaskController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, int? design = null)
    {
        IQueryable<ProductionTask> tasks = dataContext.ProductionTasks
            .Include(t => t.Order.Design)
            .Include(t => t.Prototype)
            .Where(t => t.WorkerId == user.ConfirmedId)
            .Where(t => t.Status == TaskStatus.Unknown
                     || t.Status == TaskStatus.Assigned 
                     || t.Status == TaskStatus.Working);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Order.DesignId == designId);
        }

        var groupedTasks = tasks
            .GroupBy(t => t.PrototypeId)
            .Select(g => new MultiProductionTaskResult(g).Mini());

        return Ok(groupedTasks);
    }

    [HttpGet("archive")]
    public IActionResult Archive (int? page = null, int? design = null)
    {
        IQueryable<ProductionTask> tasks = dataContext.ProductionTasks
            .Include(t => t.Order.Design)
            .Include(t => t.Prototype)
            .Where(t => t.WorkerId == user.ConfirmedId)
            .Where(t => t.Status == TaskStatus.Finished
                     || t.Status == TaskStatus.Rejected
                     || t.Status == TaskStatus.Impossible);

        if (design is int designId) {
            tasks = tasks.Where(t => t.Order.DesignId == designId);
        }

        var groupedTasks = tasks
            .GroupBy(t => t.PrototypeId)
            .Select(g => new MultiProductionTaskResult(g).Mini());

        return Ok(groupedTasks);
    }
}
