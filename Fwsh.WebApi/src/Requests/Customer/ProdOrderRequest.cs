namespace Fwsh.WebApi.Requests.Customer;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class ProdOrderRequest : Request, UpdateRequest<ProdOrder>
{
    public int Quantity { get; set; }
    public int DesignId { get; set; }
    public int FabricId { get; set; }
    public int? DecorId { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("quantity", this.Quantity).ValueInRange(1, 100);
    }

    public void ApplyTo (ProdOrder order)
    {
        order.Quantity = this.Quantity;
        order.DesignId = this.DesignId;
        order.FabricId = this.FabricId;
        order.DecorId = this.DecorId;
    }
}
