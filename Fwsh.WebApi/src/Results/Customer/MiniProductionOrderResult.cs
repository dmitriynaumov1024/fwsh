namespace Fwsh.WebApi.Results.Customer;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Results.Common;
using Fwsh.WebApi.Results.Catalog;

public class MiniProductionOrderResult : OrderResult
{
    public int Quantity { get; set; }
    public int PricePerOne { get; set; }
    public int PriceTotal => this.Quantity * this.PricePerOne;

    public MiniDesignResult Design { get; set; }

    public MiniProductionOrderResult (ProductionOrder order) : base(order)
    {
        this.Quantity = order.Quantity;
        this.PricePerOne = order.PricePerOne;
        this.Design = new MiniDesignResult(order.Design);
    }
}
