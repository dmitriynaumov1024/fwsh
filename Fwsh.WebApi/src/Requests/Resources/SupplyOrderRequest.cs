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

    public double ExpectQuantity { get; set; }
    public double ExpectPricePerUnit { get; set; }

    public double ActualQuantity { get; set; }
    public double ActualPricePerUnit { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("externalId", this.ExternalId)
            .LengthInRange(0, 20);

        validator.Property("expectQuantity", this.ExpectQuantity)
            .ValueInRange(1, 99999);

        validator.Property("expectPricePerUnit", this.ExpectPricePerUnit)
            .ValueInRange(0.01, 99999);

        validator.Property("actualQuantity", this.ActualQuantity)
            .ValueInRange(1, 99999);

        validator.Property("actualPricePerUnit", this.ActualPricePerUnit)
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
        order.ExpectQuantity = this.ExpectQuantity;
        order.ExpectPricePerUnit = this.ExpectPricePerUnit;
        order.ActualQuantity = this.ActualQuantity;
        order.ActualPricePerUnit = this.ActualPricePerUnit;
    }

}
