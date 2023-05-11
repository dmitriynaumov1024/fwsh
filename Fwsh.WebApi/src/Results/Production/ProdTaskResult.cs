namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class ProdTaskResult : Result, IResultBuilder<ProdTaskResult>
{
    private ResultBuildingContext rbcontext;
    private ProdTask task;

    public int Id { get; set; }
    public int? OrderId { get; set; }
    public int PrototypeId { get; set; }
    public int? WorkerId { get; set; }
    public int Payment { get; set; }
    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public ProdOrderResult Order { get; set; }
    public TaskPrototypeResult Prototype { get; set; }
    public WorkerResult Worker { get; set; }

    public ProdTaskResult () { }

    public ProdTaskResult (ProdTask task, ResultBuildingContext rbcontext = null)
    {
        this.rbcontext = rbcontext ?? new ResultBuildingContext();
        this.task = task;
    }
    
    public ProdTaskResult Mini () 
    {
        var result = new ProdTaskResult() {
            Id = task.Id,
            OrderId = task.Furniture?.OrderId,
            PrototypeId = task.PrototypeId,
            WorkerId = task.WorkerId,
            Payment = task.Payment,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            StartedAt = task.StartedAt,
            FinishedAt = task.FinishedAt
        };

        if (task.Prototype != null && !rbcontext.Contains(task.Prototype)) {
            rbcontext.Add(task.Prototype);
            result.Prototype = new TaskPrototypeResult(task.Prototype, rbcontext).Mini();
        }

        return result;
    }

    public ProdTaskResult ForCustomer ()
    {
        return null;
    }

    public ProdTaskResult ForWorker ()
    {
        var result = new ProdTaskResult() {
            Id = task.Id,
            OrderId = task.Furniture.OrderId,
            PrototypeId = task.PrototypeId,
            WorkerId = task.WorkerId,
            Payment = task.Payment,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            StartedAt = task.StartedAt,
            FinishedAt = task.FinishedAt
        };

        if (task.Furniture.Order != null) 
            result.Order = new ProdOrderResult(task.Furniture.Order).ForWorker();

        if (task.Prototype != null && !rbcontext.Contains(task.Prototype)) {
            rbcontext.Add(task.Prototype);
            result.Prototype = new TaskPrototypeResult(task.Prototype, rbcontext).ForWorker();
        }

        return result;
    }

    public ProdTaskResult ForManager ()
    {
        var result = ForWorker();

        if (task.Worker != null)
            result.Worker = new WorkerResult(task.Worker);
            
        return result;
    }
}
