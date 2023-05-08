namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class ProdFurniture 
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public int DesignId { get; set; }
    public int? FabricId { get; set; }
    public int? DecorId { get; set; }

    public string Status { get; set; } = OrderStatus.Unknown;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public virtual ProdOrder Order { get; set; }
    public virtual Design Design { get; set; }
    public virtual Resource Fabric { get; set; }
    public virtual Resource Decor { get; set; }

    public virtual ICollection<ProdTask> Tasks { get; set; }

    public ProdFurniture()
    {
        this.Tasks = new HashSet<ProdTask>();
    }
}

