namespace Fwsh.WebApi.Requests.Manager;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Validation;

public class SupplyOrderRequest : Request, 
    CreationRequest<SupplyOrder>, UpdateRequest<SupplyOrder>
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
            .NotNull().LengthInRange(0, 20);

        validator.Property("expectQuantity", this.ExpectQuantity)
            .NotNull().ValueInRange(0, 100000);

        validator.Property("expectPricePerUnit", this.ExpectPricePerUnit)
            .NotNull().ValueInRange(0, 10000);

        validator.Property("actualQuantity", this.ActualQuantity)
            .NotNull().ValueInRange(0, 100000);

        validator.Property("actualPricePerUnit", this.ActualPricePerUnit)
            .NotNull().ValueInRange(0, 10000);

        validator.Property("expectPricePerUnit", this.ExpectPricePerUnit * this.ExpectQuantity)
            .NotNull().ValueInRange(0, 1000000);

        validator.Property("actualPricePerUnit", this.ActualPricePerUnit * this.ActualQuantity)
            .NotNull().ValueInRange(0, 1000000);
    }

    public SupplyOrder Create()
    {
        var order = new SupplyOrder();
        this.ApplyTo(order);
        return order;
    }

    public void ApplyTo (SupplyOrder order)
    {
        order.ItemId = this.ItemId;
        order.SupplierId = this.SupplierId;
        order.ExternalId = this.ExternalId;
        order.ExpectQuantity = this.ExpectQuantity;
        order.ExpectPricePerUnit = this.ExpectPricePerUnit;
        order.ActualQuantity = this.ActualQuantity;
        order.ActualPricePerUnit = this.ActualPricePerUnit;
    }
}
