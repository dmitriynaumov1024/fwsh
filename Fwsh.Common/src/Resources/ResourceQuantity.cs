namespace Fwsh.Common;

using System;

public class ResourceQuantity
{
    public int Id { get; set; }
    public int? ItemId { get; set; }
    public string SlotName { get; set; } = SlotNames.Unknown;
    public string ItemName { get; set; }
    public double ExpectQuantity { get; set; }
    public double ActualQuantity { get; set; }

    public virtual Resource Item { get; set; }

    public int ExpectPrice => 
        (int)Math.Ceiling((this.Item?.PricePerUnit ?? this.DefaultPrice()) * this.ExpectQuantity);

    public int ActualPrice => (this.Item == null)? 0 :
        (int)Math.Ceiling(this.Item.PricePerUnit * this.ActualQuantity);
}
