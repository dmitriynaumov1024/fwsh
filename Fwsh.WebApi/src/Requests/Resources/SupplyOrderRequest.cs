namespace Fwsh.WebApi.Requests.Resources;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public class SupplyOrderRequest : Request, CreationRequest<SupplyOrder>, UpdateRequest<SupplyOrder>
{
    public int SupplierId { get; set; }
    public int ItemId { get; set; }
    public string ExternalId { get; set; }

    public double RequestQuantity { get; set; }
    public double RequestPricePerUnit { get; set; }

    public double ResultQuantity { get; set; }
    public double ResultPricePerUnit { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("externalId", this.ExternalId)
            .LengthInRange(0, 20);

        validator.Property("requestQuantity", this.RequestQuantity)
            .ValueInRange(1, 99999);

        validator.Property("requestPricePerUnit", this.RequestPricePerUnit)
            .ValueInRange(0.01, 99999);

        validator.Property("resultQuantity", this.ResultQuantity)
            .ValueInRange(1, 99999);

        validator.Property("resultPricePerUnit", this.ResultPricePerUnit)
            .ValueInRange(0.01, 99999);
    }

    public SupplyOrder Create()
    {
        SupplyOrder order = new SupplyOrder();
        this.ApplyTo(order);
        return order;
    }

    public void ApplyTo (SupplyOrder order)
    {
        order.SupplierId = this.SupplierId;
        order.ItemId = this.ItemId;
        order.ExternalId = this.ExternalId;
        order.RequestQuantity = this.RequestQuantity;
        order.RequestPricePerUnit = this.RequestPricePerUnit;
        order.ResultQuantity = this.ResultQuantity;
        order.ResultPricePerUnit = this.ResultPricePerUnit;
    }

}
