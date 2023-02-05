namespace Fwsh.Common;

using System;

public class TaskFabric : ResourceQuantity<double, Fabric>
{
    // fabric has special behavior because 
    // it can be determined by order 
    public int TaskId { get; set; }
    public int? FabricId { get; set; }
    public bool DeterminedByOrder => this.Item == null && this.FabricId == null;

    public override int CalculateResourcePrice()
    {
        if (this.DeterminedByOrder) return 0;
        return (int)Math.Ceiling(this.Quantity * this.Item.PricePerUnit);
    }
}
