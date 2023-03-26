namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class ProductionTask
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PrototypeId { get; set; }
    public int? WorkerId { get; set; }
    
    public string Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public virtual TaskPrototype Prototype { get; set; }
    public virtual ProductionOrder Order { get; set; }
    public virtual Worker Worker { get; set; }
}
