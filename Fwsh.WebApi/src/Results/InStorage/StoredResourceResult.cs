namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using Fwsh.Common;

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

}
