namespace Fwsh.WebApi.Results.Worker;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Results.Common;

public class WorkerProfileResult : PersonResult
{
    public List<string> Roles { get; set; }

    public WorkerProfileResult (Worker worker) : base(worker) 
    { 
        this.Roles = worker.Roles
            .Select(r => r.RoleName).ToList();
    }
}
