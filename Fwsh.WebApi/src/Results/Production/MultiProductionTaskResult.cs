namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class MultiProductionTaskResult : Result, IResultBuilder<MultiProductionTaskResult>
{
    private IEnumerable<ProductionTask> tasks;
    private ProductionTask task;
    private int count;

    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public int PrototypeId { get; set; }
    public int? WorkerId { get; set; }
    public IEnumerable<int> Ids { get; set; }
    public IEnumerable<string> Status { get; set; }

    public ProductionOrderResult Order { get; set; }
    public TaskPrototypeResult Prototype { get; set; }
    public WorkerResult Worker { get; set; }

    public MultiProductionTaskResult () { }

    public MultiProductionTaskResult (IEnumerable<ProductionTask> tasks)
    {
        this.tasks = tasks;
        this.task = tasks.First();
        this.count = tasks.Count();
    }
    
    public MultiProductionTaskResult Mini () 
    {
        var result = new MultiProductionTaskResult() {
            Quantity = count,
            OrderId = task.OrderId,
            PrototypeId = task.PrototypeId,
            WorkerId = task.WorkerId,
            Ids = tasks.Select(t => t.Id).ToList(),
            Status = new HashSet<string>(tasks.Select(t => t.Status))
        };

        if (task.Prototype != null) 
            result.Prototype = new TaskPrototypeResult(task.Prototype).Mini();

        if (task.Worker != null)
            result.Worker = new WorkerResult(task.Worker);

        return result;
    }

    public MultiProductionTaskResult ForCustomer ()
    {
        return null;
    }

    public MultiProductionTaskResult ForWorker ()
    {
        var result = new MultiProductionTaskResult() {
            Quantity = count,
            OrderId = task.OrderId,
            PrototypeId = task.PrototypeId,
            WorkerId = task.WorkerId,
            Ids = tasks.Select(t => t.Id).ToList(),
            Status = new HashSet<string>(tasks.Select(t => t.Status))
        };

        if (task.Order != null) 
            result.Order = new ProductionOrderResult(task.Order).ForWorker();

        if (task.Prototype != null) 
            result.Prototype = new TaskPrototypeResult(task.Prototype).ForWorker();

        return result;
    }

    public MultiProductionTaskResult ForManager ()
    {
        var result = ForWorker();

        if (task.Worker != null)
            result.Worker = new WorkerResult(task.Worker);
            
        return result;
    }
}
