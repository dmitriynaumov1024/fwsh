namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class SupplierResult : PersonResult
{
    public string OrgName { get; set; }

    public SupplierResult (Supplier supplier) : base(supplier) 
    { 
        this.OrgName = supplier.OrgName;
    }
}
