namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class MultiProdTaskResult : Result, IResultBuilder<MultiProdTaskResult>
{
    private IEnumerable<ProdTask> tasks;
    private ProdTask task;
    private int count;

    public int Quantity { get; set; }
    public int? OrderId { get; set; }
    public int PrototypeId { get; set; }
    public int? WorkerId { get; set; }
    public IEnumerable<int> Ids { get; set; }
    public IEnumerable<string> Status { get; set; }

    public ProdOrderResult Order { get; set; }
    public TaskPrototypeResult Prototype { get; set; }
    public WorkerResult Worker { get; set; }

    public MultiProdTaskResult () { }

    public MultiProdTaskResult (IEnumerable<ProdTask> tasks)
    {
        this.tasks = tasks;
        this.task = tasks.First();
        this.count = tasks.Count();
    }
    
    public MultiProdTaskResult Mini () 
    {
        var result = new MultiProdTaskResult() {
            Quantity = count,
            OrderId = task.Furniture?.OrderId,
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

    public MultiProdTaskResult ForCustomer ()
    {
        return null;
    }

    public MultiProdTaskResult ForWorker ()
    {
        var result = new MultiProdTaskResult() {
            Quantity = count,
            OrderId = task.Furniture?.OrderId,
            PrototypeId = task.PrototypeId,
            WorkerId = task.WorkerId,
            Ids = tasks.Select(t => t.Id).ToList(),
            Status = new HashSet<string>(tasks.Select(t => t.Status))
        };

        if (task.Furniture.Order != null) 
            result.Order = new ProdOrderResult(task.Furniture.Order).ForWorker();

        if (task.Prototype != null) 
            result.Prototype = new TaskPrototypeResult(task.Prototype).ForWorker();

        return result;
    }

    public MultiProdTaskResult ForManager ()
    {
        var result = ForWorker();

        if (task.Worker != null)
            result.Worker = new WorkerResult(task.Worker);
            
        return result;
    }
}
