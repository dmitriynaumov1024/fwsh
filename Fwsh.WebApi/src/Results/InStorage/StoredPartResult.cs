namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class StoredPartResult : StoredResourceResult<int, Part>, IResultBuilder<StoredPartResult>
{
    private StoredPart resource;

    public PartResult Item { get; set; }

    public StoredPartResult () { }

    public StoredPartResult (StoredPart resource) 
    { 
        this.resource = resource;
    }

    public StoredPartResult Mini ()
    {
        return new StoredPartResult() {
            Id = resource.Id,
            InStock = resource.InStock,
            NormalStock = resource.NormalStock,
            SupplierId = resource.SupplierId,
            ExternalId = resource.ExternalId,
            RefillPeriodDays = resource.RefillPeriodDays,
            LastRefilledAt = resource.LastRefilledAt,
            LastCheckedAt = resource.LastCheckedAt,
            Item = new PartResult(resource.Item)
        };
    }

    public StoredPartResult ForCustomer () 
    {
        return null;
    }

    public StoredPartResult ForWorker () 
    {
        var result = Mini();
        result.Supplier = new SupplierResult(resource.Supplier);

        return result;
    }

    public StoredPartResult ForManager ()
    {
        return this.ForWorker();
    }
}
