namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class RepairTaskResult : Result, IResultBuilder<RepairTaskResult>
{
    private ResultBuildingContext rbcontext;
    private RepairTask task;

    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? WorkerId { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Role { get; set; }
    public int Payment { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public RepairOrderResult Order { get; set; }
    public WorkerResult Worker { get; set; }

    public RepairTaskResult () { }

    public RepairTaskResult (RepairTask task, ResultBuildingContext rbcontext = null)
    {
        this.rbcontext = rbcontext ?? new ResultBuildingContext();
        this.task = task;
    }
    
    public RepairTaskResult Mini () 
    {
        var result = new RepairTaskResult() {
            Id = task.Id,
            OrderId = task.OrderId,
            WorkerId = task.WorkerId,
            Description = task.Description,
            Status = task.Status,
            Role = task.Role,
            Payment = task.Payment,
            CreatedAt = task.CreatedAt,
            StartedAt = task.StartedAt,
            FinishedAt = task.FinishedAt
        };

        return result;
    }

    public RepairTaskResult ForCustomer ()
    {
        return null;
    }

    public RepairTaskResult ForWorker ()
    {
        var result = Mini();

        if (task.Order != null) 
            result.Order = new RepairOrderResult(task.Order).ForWorker();

        return result;
    }

    public RepairTaskResult ForManager ()
    {
        var result = Mini();

        if (task.Order != null) 
            result.Order = new RepairOrderResult(task.Order).ForManager();

        if (task.Worker != null)
            result.Worker = new WorkerResult(task.Worker);
            
        return result;
    }
}
