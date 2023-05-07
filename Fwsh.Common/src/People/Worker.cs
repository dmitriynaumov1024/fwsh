namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class Worker : Person
{
    public virtual List<string> Roles { get; set; }

    public virtual ICollection<WorkerPaycheck> Paychecks { get; set; }

    public virtual ICollection<ProdTask> ProdTasks { get; set; }
    public virtual ICollection<RepairTask> RepairTasks { get; set; }

    public Worker() : base()
    {
        this.Roles = new List<string>();

        this.Paychecks = new HashSet<WorkerPaycheck>();
        
        this.ProdTasks = new HashSet<ProdTask>();
        this.RepairTasks = new HashSet<RepairTask>();
    }

}
