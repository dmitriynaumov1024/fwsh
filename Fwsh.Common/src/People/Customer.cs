namespace Fwsh.Common;

using System;
using System.Collections.Generic;
using System.Linq;

public class Customer : Person
{
    public bool IsOrganization { get; set; }
    public string OrgName { get; set; }

    public int DiscountPercent { get; set; }

    public ICollection<ProdOrder> ProdOrders { get; set; }
    public ICollection<RepairOrder> RepairOrders { get; set; }

    public Customer() : base()
    {
        this.ProdOrders = new HashSet<ProdOrder>();
        this.RepairOrders = new HashSet<RepairOrder>();
    }

    public int CalculateDiscountPercent()
    {
        double discountPerOneOrder = this.IsOrganization ? 0.25 : 2.0;

        double discount = this.ProdOrders
            .Where(order => order.Status == OrderStatus.Received)
            .Count() * discountPerOneOrder;

        return (int)Math.Clamp(discount, 0, PriceFormation.MaxDiscountPercent);
    }

    public void UpdateDiscountPercent()
    {
        this.DiscountPercent = this.CalculateDiscountPercent();
    }
}
