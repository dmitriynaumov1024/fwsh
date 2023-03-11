namespace Fwsh.WebApi.Results.Customer;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Results.Common;

public class MiniRepairOrderResult : OrderResult
{
    public int Price { get; set; }
    public int Prepayment { get; set; }

    public List<string> PhotoUrls { get; set; }

    public MiniRepairOrderResult (RepairOrder order) : base(order)
    {
        this.Price = order.Price;
        this.Prepayment = order.Prepayment;

        this.PhotoUrls = order.Photos
            .OrderBy(p => p.Position)
            .Select(p => p.Url).ToList();
    }
}
