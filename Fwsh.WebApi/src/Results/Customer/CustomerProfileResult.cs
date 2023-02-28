namespace Fwsh.WebApi.Results.Customer;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Results.Common;

public class CustomerProfileResult : PersonResult
{
    public string OrgName { get; set; }

    public bool IsOrganization => this.OrgName != null;

    public CustomerProfileResult (Customer customer) : base(customer) 
    { 
        this.OrgName = customer.OrgName;
    }
}
