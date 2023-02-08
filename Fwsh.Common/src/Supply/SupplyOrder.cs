namespace Fwsh.Common;

using System;

public class SupplyOrder<TCount, TResource> 
where TResource : Resource
{
    public int Id { get; set; }

    public int SupplierId { get; set; }
    public int ItemId { get; set; }
    public string ExternalItemId { get; set; }
    public int Quantity { get; set; }
    public double PricePerUnit { get; set; }

    public string Status { get; set; } = SupplyOrderStatus.Unknown;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReceivedAt { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual TResource Item { get; set; }

    public SupplyOrder() { }

    public SupplyOrder (StoredResource<TCount, TResource> resource)
    {
        this.Item = resource.Item;
        this.ItemId = resource.Id;
        this.ExternalItemId = resource.ExternalId;
        this.Supplier = resource.Supplier;
        this.SupplierId = resource.SupplierId;
        this.Quantity = resource.NormalStock - resource.InStock;
        this.PricePerUnit = resource.Item.PricePerUnit;
    }
}
