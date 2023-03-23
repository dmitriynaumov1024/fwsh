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
            Item = new FabricResult() {
                Name = resource.Item.Name,
                PricePerUnit = resource.Item.PricePerUnit
            }
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
        result.Item = new FabricResult(resource.Item);

        return result;
    }

    public StoredFabricResult ForManager ()
    {
        return this.ForWorker();
    }
}