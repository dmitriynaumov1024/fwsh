namespace Fwsh.WebApi.Results.Resources;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Results.Common;

public class SupplierResult : PersonResult
{
    public string OrgName { get; set; }

    public SupplierResult (Supplier supplier) : base(supplier) 
    { 
        this.OrgName = supplier.OrgName;
    }
}
