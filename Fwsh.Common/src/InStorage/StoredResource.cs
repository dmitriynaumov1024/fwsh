namespace Fwsh.Common;

using System;

public class StoredResource<TCount, TResource> : ResourceQuantity<TCount, TResource>
where TResource : Resource
{
    public int Id { get; set; }
    
    public int SupplierId { get; set; }
    public string ExternalId { get; set; }

    public int NormalStock { get; set; }

    public int RefillPeriodDays { get; set; }
    public DateTime LastRefilledAt { get; set; } = DateTime.UtcNow;
    public DateTime LastCheckedAt { get; set; } = DateTime.UtcNow;

    public virtual Supplier Supplier { get; set; }

    public virtual int InStock => 0;

    public override int CalculateResourcePrice()
    {
        return 0;
    }
}
