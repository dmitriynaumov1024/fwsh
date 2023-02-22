namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class CustomerResult : PersonResult
{
    public string OrgName { get; set; }

    public bool IsOrganization => this.OrgName != null;

    public CustomerResult (Customer customer) : base(customer) 
    { 
        this.OrgName = customer.OrgName;
    }
}
