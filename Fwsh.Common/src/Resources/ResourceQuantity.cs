namespace Fwsh.Common;

using System;

public class ResourceQuantity
{
    public int Id { get; set; }
    public int? ItemId { get; set; }
    public string SlotName { get; set; } = SlotNames.Unknown;
    public string ItemName { get; set; }
    public double Quantity { get; set; }
    
    public virtual Resource Item { get; set; }

    public int CalculateResourcePrice() 
    {
        if (this.Item == null) return 0;
        return (int)Math.Ceiling(this.Item.PricePerUnit * this.Quantity);
    }
}
