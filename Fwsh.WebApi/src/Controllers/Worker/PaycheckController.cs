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
[Route("worker/paychecks")]
public class PaycheckController : FwshController
{
    const int PAGESIZE = 20;

    static readonly TimeSpan MinPaycheckInterval = TimeSpan.FromDays(6);

    public PaycheckController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1)
    {
        if (page < 0) return BadRequest (new BadFieldResult("page"));
        
        var paychecks = dataContext.Workers
            .Include(w => w.Paychecks)
            .Where(w => w.Id == user.ConfirmedId)
            .SelectMany(w => w.Paychecks)
            .Where(p => p.IsReceived == false)
            .OrderByDescending(p => p.Id);

        return Ok(paychecks.Paginate(page, PAGESIZE));
    }

    [HttpGet("archive")]
    public IActionResult Archive (int page = -1) 
    {
        if (page < 0) return BadRequest (new BadFieldResult("page"));
        
        var paychecks = dataContext.Workers
            .Include(w => w.Paychecks)
            .Where(w => w.Id == user.ConfirmedId)
            .SelectMany(w => w.Paychecks)
            .Where(p => p.IsReceived)
            .OrderByDescending(p => p.Id);

        return Ok(paychecks.Paginate(page, PAGESIZE));
    }

    [HttpPost("create")]
    public IActionResult Create()
    {
        var worker = dataContext.Workers
            .Include(w => w.Paychecks)
            .Where(w => w.Id == user.ConfirmedId)
            .FirstOrDefault();

        var lastPaycheck = worker.Paychecks.OrderByDescending(p => p.Id).FirstOrDefault();

        if (lastPaycheck != null && DateTime.UtcNow - lastPaycheck.IntervalEnd < MinPaycheckInterval) {
            return BadRequest(new TryLaterResult(lastPaycheck.IntervalEnd + MinPaycheckInterval, "Too early"));
        }

        IEnumerable<ProdTask> prodTasks = dataContext.ProdTasks
            .Include(t => t.Furniture.Order)
            .Where(t => t.WorkerId == user.ConfirmedId
                && t.Furniture.Order.FinishedAt != null
                && t.Status == TaskStatus.Finished).ToList();

        IEnumerable<RepairTask> repairTasks = dataContext.RepairTasks
            .Include(t => t.Order)
            .Where(t => t.WorkerId == user.ConfirmedId
                && t.Order.FinishedAt != null
                && t.Status == TaskStatus.Finished).ToList();

        List<WorkTask> allTasks = (prodTasks as IEnumerable<WorkTask>).Concat(repairTasks).ToList();

        if (allTasks.Count == 0) {
            return BadRequest(new TryLaterResult());
        }

        var paycheck = new WorkerPaycheck() {
            WorkerId = user.ConfirmedId,
            IntervalStart = (DateTime)(lastPaycheck?.IntervalEnd ?? allTasks.Min(t => t.FinishedAt)),
            IntervalEnd = DateTime.UtcNow,
            Amount = allTasks.Sum(t => t.Payment),
            IsReceived = false
        };

        foreach (var task in allTasks) {
            task.Status = TaskStatus.Paid;
        }

        try {
            dataContext.Workers.Update(worker);
            dataContext.ProdTasks.UpdateRange(prodTasks);
            dataContext.RepairTasks.UpdateRange(repairTasks);
            dataContext.SaveChanges();
            return Ok(new SuccessResult("Successfully created paycheck"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not create paycheck"));
        }
    }

}
