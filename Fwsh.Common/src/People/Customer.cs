namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class Customer : Person
{
    public int DiscountPercent { get; set; }

    public bool IsOrganization { get; set; }
    public string OrgName { get; set; }
       
    public string Password { get; set; }

    public ICollection<ProductionOrder> ProductionOrders { get; set; }
    public ICollection<RepairOrder> RepairOrders { get; set; }

    public Customer() : base()
    {
        this.ProductionOrders = new HashSet<ProductionOrder>();
        this.RepairOrders = new HashSet<RepairOrder>();
    }
}
