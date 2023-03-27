namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class ProductionTaskResult : Result, IResultBuilder<ProductionTaskResult>
{
    private ResultBuildingContext rbcontext;
    private ProductionTask task;

    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PrototypeId { get; set; }
    public int? WorkerId { get; set; }
    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public ProductionOrderResult Order { get; set; }
    public TaskPrototypeResult Prototype { get; set; }
    public WorkerResult Worker { get; set; }

    public ProductionTaskResult () { }

    public ProductionTaskResult (ProductionTask task, ResultBuildingContext rbcontext = null)
    {
        this.rbcontext = rbcontext ?? new ResultBuildingContext();
        this.task = task;
    }
    
    public ProductionTaskResult Mini () 
    {
        var result = new ProductionTaskResult() {
            Id = task.Id,
            OrderId = task.OrderId,
            PrototypeId = task.PrototypeId,
            WorkerId = task.WorkerId,
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

    public ProductionTaskResult ForCustomer ()
    {
        return null;
    }

    public ProductionTaskResult ForWorker ()
    {
        var result = new ProductionTaskResult() {
            Id = task.Id,
            OrderId = task.OrderId,
            PrototypeId = task.PrototypeId,
            WorkerId = task.WorkerId,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            StartedAt = task.StartedAt,
            FinishedAt = task.FinishedAt
        };

        if (task.Order != null) 
            result.Order = new ProductionOrderResult(task.Order).ForWorker();

        if (task.Prototype != null && !rbcontext.Contains(task.Prototype)) {
            rbcontext.Add(task.Prototype);
            result.Prototype = new TaskPrototypeResult(task.Prototype, rbcontext).ForWorker();
        }

        return result;
    }

    public ProductionTaskResult ForManager ()
    {
        var result = ForWorker();

        if (task.Worker != null)
            result.Worker = new WorkerResult(task.Worker);
            
        return result;
    }
}
