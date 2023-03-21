namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using Fwsh.Common;

public class StoredMaterialResult : StoredResourceResult<double, Material>, IResultBuilder<StoredMaterialResult>
{
    private StoredMaterial resource;

    public MaterialResult Item { get; set; }

    public StoredMaterialResult () { }

    public StoredMaterialResult (StoredMaterial resource) 
    { 
        this.resource = resource;
    }

    public StoredMaterialResult Mini ()
    {
        return new StoredMaterialResult() {
            Id = resource.Id,
            InStock = resource.InStock,
            NormalStock = resource.NormalStock,
            SupplierId = resource.SupplierId,
            ExternalId = resource.ExternalId,
            RefillPeriodDays = resource.RefillPeriodDays,
            LastRefilledAt = resource.LastRefilledAt,
            LastCheckedAt = resource.LastCheckedAt,
            Item = new MaterialResult() {
                Name = resource.Item.Name,
                PricePerUnit = resource.Item.PricePerUnit
            }
        };
    }

    public StoredMaterialResult ForCustomer () 
    {
        return null;
    }

    public StoredMaterialResult ForWorker () 
    {
        var result = Mini();
        result.Supplier = new SupplierResult(resource.Supplier);
        result.Item = new MaterialResult(resource.Item);
        
        return result;
    }

    public StoredMaterialResult ForManager ()
    {
        return this.ForWorker();
    }
}
