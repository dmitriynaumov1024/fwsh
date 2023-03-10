namespace Fwsh.Common;

using System;

public class TaskPart : TaskResourceUsage<int, Part>
{
    public int PartId { get; set; }

    public override int CalculateResourcePrice()
    {
        return (int)Math.Ceiling(this.Quantity * this.Item.PricePerUnit);
    }
}
