namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;
using Fwsh.Common;
using Fwsh.WebApi.Results.Catalog;

public class StoredResourceResult<TCount, TResource> : Result
where TResource : Resource
{
    public int Id { get; set; }

    public int SupplierId { get; set; }
    public string ExternalId { get; set; }

    public int InStock { get; set; }
    public int NormalStock { get; set; }

    public int RefillPeriodDays { get; set; }
    public DateTime? LastRefilledAt { get; set; }
    public DateTime? LastCheckedAt { get; set; }

    public SupplierResult Supplier { get; set; }

    public StoredResourceResult () { }

    public StoredResourceResult (StoredResource<TCount, TResource> resource, bool full = false) 
    { 
        this.Id = resource.Id;
        this.InStock = resource.InStock;
        this.NormalStock = resource.NormalStock;

        if (full) {
            this.Supplier = new SupplierResult(resource.Supplier);
            this.ExternalId = resource.ExternalId;
            this.RefillPeriodDays = resource.RefillPeriodDays;
            this.LastRefilledAt = resource.LastRefilledAt;
            this.LastCheckedAt = resource.LastCheckedAt;
        }
    }

}
