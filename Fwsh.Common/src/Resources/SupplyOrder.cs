namespace Fwsh.Common;

using System;

public class SupplyOrder
{   
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int ItemId { get; set; }
    public string ExternalId { get; set; }

    public double RequestQuantity { get; set; }
    public double RequestPricePerUnit { get; set; }

    public double ResultQuantity { get; set; }
    public double ResultPricePerUnit { get; set; }

    public string Status { get; set; } = OrderStatus.Unknown;
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReceivedAt { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual StoredResource Item { get; set; }
}
