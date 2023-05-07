namespace Fwsh.Common;

using System;

public class StoredResource : Resource
{   
    public int? SupplierId { get; set; }
    public string ExternalId { get; set; }

    public double InStock { get; set; }
    public double NormalStock { get; set; }

    public int RefillPeriodDays { get; set; }
    public DateTime LastRefilledAt { get; set; } = DateTime.UtcNow;
    public DateTime LastCheckedAt { get; set; } = DateTime.UtcNow;

    public virtual Supplier Supplier { get; set; }

    public virtual bool IsTimeToRefill => 
        (DateTime.UtcNow - this.LastRefilledAt).Days >= this.RefillPeriodDays;

    public virtual bool NeedsRefill => 
        this.IsTimeToRefill ? 
        100 * this.InStock / this.NormalStock < 60 :
        100 * this.InStock / this.NormalStock < 30; 

}
