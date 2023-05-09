namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class CustomerResult : PersonResult
{
    public int DiscountPercent { get; set; }
    public string OrgName { get; set; }

    public bool IsOrganization { get; set; }

    public CustomerResult (Customer customer) : base(customer) 
    { 
        this.DiscountPercent = customer.DiscountPercent;
        this.OrgName = customer.OrgName;
        this.IsOrganization = customer.IsOrganization;
    }
}
