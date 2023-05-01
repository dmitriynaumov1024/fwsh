namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using Fwsh.Common;

public class StoredFabricResult : StoredResourceResult<double, Fabric>, IResultBuilder<StoredFabricResult>
{
    private StoredFabric resource;

    public FabricResult Item { get; set; }

    public StoredFabricResult () { }

    public StoredFabricResult (StoredFabric resource) 
    { 
        this.resource = resource;
    }   

    public StoredFabricResult Mini ()
    {
        return new StoredFabricResult() {
            Id = resource.Id,
            InStock = resource.InStock,
            NormalStock = resource.NormalStock,
            SupplierId = resource.SupplierId,
            ExternalId = resource.ExternalId,
            RefillPeriodDays = resource.RefillPeriodDays,
            LastRefilledAt = resource.LastRefilledAt,
            LastCheckedAt = resource.LastCheckedAt,
            NeedsRefill = resource.NeedsRefill,
            IsTimeToRefill = resource.IsTimeToRefill,
            Item = new FabricResult(resource.Item)
        };
    }

    public StoredFabricResult ForCustomer () 
    {
        return null;
    }

    public StoredFabricResult ForWorker () 
    {
        var result = Mini();
        result.Supplier = new SupplierResult(resource.Supplier);

        return result;
    }

    public StoredFabricResult ForManager ()
    {
        return this.ForWorker();
    }
}
