namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class RepairTask
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? WorkerId { get; set; }
    
    public string RoleName { get; set; }
    public int Payment { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public virtual RepairOrder Order { get; set; }
    public virtual Worker Worker { get; set; }
}
