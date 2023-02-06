namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class Worker : Person
{
    public string Password { get; set; }

    public virtual ICollection<WorkerPaycheck> Paychecks { get; set; }
    public virtual ICollection<WorkerRole> Roles { get; set; }

    public virtual ICollection<ProductionTask> ProductionTasks { get; set; }
    public virtual ICollection<RepairTask> RepairTasks { get; set; }

    public Worker() : base()
    {
        this.Paychecks = new HashSet<WorkerPaycheck>();
        this.Roles = new HashSet<WorkerRole>();
        this.ProductionTasks = new HashSet<ProductionTask>();
        this.RepairTasks = new HashSet<RepairTask>();
    }

}
