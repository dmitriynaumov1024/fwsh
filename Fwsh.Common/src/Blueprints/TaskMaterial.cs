namespace Fwsh.Common;

using System;

public class TaskMaterial : ResourceQuantity<double, Material>
{
    // some Materials, like Fabrics, can also be determined by order 
    public int TaskId { get; set; }
    public int? MaterialId { get; set; }
    public bool DeterminedByOrder => this.Item == null && this.MaterialId == null;

    public override int CalculateResourcePrice()
    {
        if (this.DeterminedByOrder) return 0;
        return (int)Math.Ceiling(this.Quantity * this.Item.PricePerUnit);
    }
}
