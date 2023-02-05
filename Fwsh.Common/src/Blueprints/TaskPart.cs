namespace Fwsh.Common;

using System;

public class TaskPart : ResourceQuantity<int, Part>
{
    public int TaskId { get; set; }
    public int PartId { get; set; }

    public override int CalculateResourcePrice()
    {
        return (int)Math.Ceiling(this.Quantity * this.Item.PricePerUnit);
    }
}
