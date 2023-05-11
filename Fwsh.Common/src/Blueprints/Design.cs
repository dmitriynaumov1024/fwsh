namespace Fwsh.Common;

using System;
using System.Collections.Generic;
using System.Linq;

public class Design 
{
    public int Id { get; set; }

    public string NameKey { get; set; }
    public string DisplayName { get; set; }
    public string Type { get; set; } = FurnitureTypes.Unknown;

    public bool IsVisible { get; set; }
    public bool IsTransformable { get; set; }

    public Dimensions DimCompact { get; set; }
    public Dimensions DimExpanded { get; set; }

    public double FabricUsage { get; set; }
    public double DecorUsage { get; set; }

    public string Description { get; set; }
    public int Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime RecalculatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<TaskPrototype> Tasks { get; set; }
    public virtual IReadOnlyList<string> PhotoUrls { get; set; }

    public Design()
    {
        this.Tasks = new HashSet<TaskPrototype>();
        this.PhotoUrls = new List<string>();
    }

    public void UpdatePrice()
    {
        this.Price = (this.CalculateResourcePrice() + this.CalculatePayment()).WithMargin();
        this.RecalculatedAt = DateTime.UtcNow;
    }

    public void UpdateResourceQuantities()
    {
        this.FabricUsage = this.Tasks
            .Sum(task => task.Fabrics.Where(f => f.SlotName == SlotNames.Fabric).Sum(f => f.Quantity));

        this.DecorUsage = this.Tasks
            .Sum(task => task.Materials.Where(m => m.SlotName == SlotNames.Decor).Sum(m => m.Quantity));
    }

    public int CalculateResourcePrice()
    {
        return this.Tasks.Sum(task => task.CalculateResourcePrice());
    }

    public int CalculatePayment()
    {
        return this.Tasks.Sum(task => task.Payment);
    }
}

