namespace Fwsh.WebApi.Results.Customer;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Results.Common;

public class CustomerProfileResult : PersonResult
{
    public int DiscountPercent { get; set; }
    public string OrgName { get; set; }

    public bool IsOrganization => this.OrgName != null;

    public CustomerProfileResult (Customer customer) : base(customer) 
    { 
        this.DiscountPercent = customer.DiscountPercent;
        this.OrgName = customer.OrgName;
    }
}
