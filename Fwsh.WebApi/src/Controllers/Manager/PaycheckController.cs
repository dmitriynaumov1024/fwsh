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
[Route("manager/paychecks")]
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
        
        var workers = dataContext.Workers
            .Include(w => w.Paychecks.Where(p => !p.IsReceived))
            .Where(w => w.Paychecks.Count() > 0);

        return Ok(workers.Paginate(page, PAGESIZE, w => new WorkerResult(w)));
    }

    [HttpGet("archive")]
    public IActionResult Archive (int page = -1) 
    {
        if (page < 0) return BadRequest (new BadFieldResult("page"));
        
        var paychecks = dataContext.Workers
            .Include(w => w.Paychecks)
            .SelectMany(w => w.Paychecks)
            .Where(p => p.IsReceived)
            .OrderByDescending(p => p.Id);

        return Ok(paychecks.Paginate(page, PAGESIZE));
    }

    [HttpPost("create")]
    public IActionResult Create (int? workerId = null)
    {
        if (workerId == null) return NotFound(new BadFieldResult("workerId"));

        try {

        var worker = dataContext.Workers
            .Include(w => w.Paychecks)
            .Where(w => w.Id == workerId)
            .FirstOrDefault();

        var lastPaycheck = worker.Paychecks.OrderByDescending(p => p.Id).FirstOrDefault();

        if (lastPaycheck != null && DateTime.UtcNow - lastPaycheck.IntervalEnd < MinPaycheckInterval) {
            return BadRequest(new TryLaterResult(lastPaycheck.IntervalEnd + MinPaycheckInterval, "Too early"));
        }

        IEnumerable<ProdTask> prodTasks = dataContext.ProdTasks
            .Include(t => t.Furniture.Order)
            .Where(t => t.WorkerId == workerId
                && t.Furniture.Order.FinishedAt != null
                && t.Status == TaskStatus.Finished).ToList();

        IEnumerable<RepairTask> repairTasks = dataContext.RepairTasks
            .Include(t => t.Order)
            .Where(t => t.WorkerId == workerId
                && t.Order.FinishedAt != null
                && t.Status == TaskStatus.Finished).ToList();

        List<WorkTask> allTasks = (prodTasks as IEnumerable<WorkTask>).Union(repairTasks).ToList();

        if (allTasks.Count == 0) {
            return BadRequest(new TryLaterResult(DateTime.UtcNow + MinPaycheckInterval));
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

    [HttpPost("create-bonus")]
    public IActionResult CreateBonus (int? workerId = null, int amount = 0)
    {
        if (amount < 1) 
            return BadRequest(new BadFieldResult("amount"));

        if (workerId == null) 
            return NotFound(new BadFieldResult("workerId"));

        var worker = dataContext.Workers
            .Where(w => w.Id == workerId)
            .FirstOrDefault();

        if (worker == null) 
            return NotFound(new BadFieldResult("workerId"));

        var paycheck = new WorkerPaycheck() {
            WorkerId = (int)workerId,
            Amount = amount,
            IntervalStart = DateTime.UtcNow,
            IntervalEnd = DateTime.UtcNow
        };

        try {
            worker.Paychecks.Add(paycheck);
            dataContext.Workers.Update(worker);
            dataContext.SaveChanges();
            return Ok(new SuccessResult("Successfully created bonus paycheck"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not create bonus paycheck"));
        }
    }

    [HttpPost("confirm-payment")]
    public IActionResult ConfirmPayment (int? workerId = null, int? paycheckId = null)
    {
        if (workerId == null) 
            return NotFound(new BadFieldResult("workerId"));
        
        if (paycheckId == null) 
            return NotFound(new BadFieldResult("paycheckId"));

        var worker = dataContext.Workers
            .Include(w => w.Paychecks.Where(p => p.Id == paycheckId && !p.IsReceived))
            .FirstOrDefault(w => w.Id == workerId);

        if (worker == null) 
            return NotFound(new BadFieldResult("workerId"));

        var paycheck = worker.Paychecks.FirstOrDefault();

        if (paycheck == null)
            return NotFound(new BadFieldResult("paycheckId"));

        try {
            paycheck.IsReceived = true;
            dataContext.Workers.Update(worker);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully confirmed payment"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not confirm payment of paycheck"));
        }
    }

}
