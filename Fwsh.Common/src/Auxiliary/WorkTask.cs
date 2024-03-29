namespace Fwsh.Common;

using System;
using System.Collections.Generic;
using System.Linq;

public abstract class WorkTask
{
    public int Id { get; set; }
    public int? WorkerId { get; set; }
    
    public int Payment { get; set; }

    public string Status { get; set; }
    public bool IsActive { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public virtual Worker Worker { get; set; }

    public virtual ICollection<ResourceQuantity> Resources { get; set; }

    public int ExpectPrice => 
        this.Resources.Sum(res => res.ExpectPrice);
    
    public int ActualPrice => 
        this.Resources.Sum(res => res.ActualPrice);

    public WorkTask() 
    {
        this.Resources = new HashSet<ResourceQuantity>();
    }

    public bool TrySetStatus (string status)
    {
        if (! TaskStatus.Contains(status)) return false;

        this.Status = status;

        if (status == TaskStatus.Working)
            this.StartedAt = DateTime.UtcNow;

        if (status == TaskStatus.Finished)
            this.FinishedAt = DateTime.UtcNow;
        else 
            this.FinishedAt = null;

        return true;
    }
}
