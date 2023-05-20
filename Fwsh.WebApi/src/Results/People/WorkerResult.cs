namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class WorkerResult : PersonResult
{
    public IReadOnlyList<string> Roles { get; set; }
    public ICollection<WorkerPaycheck> Paychecks { get; set; }

    public WorkerResult (Worker worker) : base(worker) 
    { 
        this.Roles = worker.Roles;
        
        if (worker.Paychecks != null) 
            this.Paychecks = worker.Paychecks.ToList();
    }
}
