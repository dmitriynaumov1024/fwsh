namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class WorkerResult : PersonResult
{
    public List<string> Roles { get; set; }

    public WorkerResult (Worker worker) : base(worker) 
    { 
        this.Roles = worker.Roles.ToList();
    }
}
