namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class StoredResourceResult : ResourceResult
{
    public int? SupplierId { get; set; }
    public string ExternalId { get; set; }

    public double InStock { get; set; }
    public double NormalStock { get; set; }

    public int RefillPeriodDays { get; set; }
    public DateTime LastRefilledAt { get; set; }
    public DateTime LastCheckedAt { get; set; }

    public bool IsTimeToRefill { get; set; }
    public bool NeedsRefill { get; set; }

    public SupplierResult Supplier { get; set; }

    public StoredResourceResult () { }

    public StoredResourceResult (StoredResource res) : base(res)
    { 
        this.SupplierId = res.SupplierId;
        this.ExternalId = res.ExternalId;
        this.InStock = res.InStock;
        this.NormalStock = res.NormalStock;
        this.RefillPeriodDays = res.RefillPeriodDays;
        this.LastRefilledAt = res.LastRefilledAt;
        this.LastCheckedAt = res.LastCheckedAt;
        this.IsTimeToRefill = res.IsTimeToRefill;
        this.NeedsRefill = res.NeedsRefill;
        
        if (res.Supplier != null) 
            this.Supplier = new SupplierResult(res.Supplier);
    }
}
