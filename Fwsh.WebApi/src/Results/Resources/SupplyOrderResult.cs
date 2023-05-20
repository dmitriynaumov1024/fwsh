namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class SupplyOrderResult : Result
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int ItemId { get; set; }
    public string ExternalId { get; set; }
    public double ExpectQuantity { get; set; }
    public double ExpectPricePerUnit { get; set; }
    public double ActualQuantity { get; set; }
    public double ActualPricePerUnit { get; set; }
    public string Status { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReceivedAt { get; set; }
    public SupplierResult Supplier { get; set; }
    public StoredResourceResult Item { get; set; }

    public SupplyOrderResult () { }

    public SupplyOrderResult (SupplyOrder order)
    {
        if (order.Item != null)
            this.Item = new StoredResourceResult(order.Item);

        if (order.Supplier != null)
            this.Supplier = new SupplierResult(order.Supplier);

        this.Id = order.Id;
        this.SupplierId = order.SupplierId;
        this.ItemId = order.ItemId;
        this.ExternalId = order.ExternalId;
        this.ExpectQuantity = order.ExpectQuantity;
        this.ExpectPricePerUnit = order.ExpectPricePerUnit;
        this.ActualQuantity = order.ActualQuantity;
        this.ActualPricePerUnit = order.ActualPricePerUnit;
        this.Status = order.Status;
        this.IsActive = order.IsActive;
        this.CreatedAt = order.CreatedAt;
        this.ReceivedAt = order.ReceivedAt;
    }
}
