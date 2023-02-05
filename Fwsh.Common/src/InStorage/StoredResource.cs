namespace Fwsh.Common;

using System;

public class StoredResource<TCount, TResource> : ResourceQuantity<TCount, TResource>
{
    public int Id { get; set; }
    
    public int NormalStock { get; set; }

    public int RefillPeriodDays { get; set; }
    public DateTime LastRefilledAt { get; set; } = DateTime.UtcNow;
    public DateTime LastCheckedAt { get; set; } = DateTime.UtcNow;

    public override int CalculateResourcePrice()
    {
        return 0;
    }
}
