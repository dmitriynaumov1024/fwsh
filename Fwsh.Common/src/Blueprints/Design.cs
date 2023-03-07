namespace Fwsh.Common;

using System;
using System.Collections.Generic;
using System.Linq;

public class Design 
{
    public int Id { get; set; }

    public string NameKey { get; set; }

    public string Type { get; set; } = FurnitureTypes.Unknown;

    public string DisplayName { get; set; }
    public string Description { get; set; }

    public bool IsTransformable { get; set; }
    public Dimensions DimCompact { get; set; }
    public Dimensions DimExpanded { get; set; }

    public double FabricQuantity { get; set; }
    public double DecorMaterialQuantity { get; set; }

    public int Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PriceRecalculatedAt { get; set; }

    public virtual ICollection<TaskPrototype> TaskPrototypes { get; set; }
    public virtual ICollection<DesignPhoto> Photos { get; set; }

    public string DimCompactString { 
        get { 
            return this.DimCompact?.ToConventionalString(); 
        }
        set { 
            if (value == null || value == String.Empty) return;
            this.DimCompact = Dimensions.Parse(value); 
        }
    }

    public string DimExpandedString { 
        get { 
            return this.DimExpanded?.ToConventionalString(); 
        }
        set { 
            if (value == null || value == String.Empty) return;
            this.DimExpanded = Dimensions.Parse(value); 
        }
    }

    public Design()
    {
        this.TaskPrototypes = new HashSet<TaskPrototype>();
        this.Photos = new HashSet<DesignPhoto>();
    }

    public void UpdatePrice()
    {
        this.Price = this.CalculateResourcePrice() + this.CalculatePayment();
        this.PriceRecalculatedAt = DateTime.UtcNow;
    }

    public void UpdateResourceQuantities()
    {
        this.FabricQuantity = this.TaskPrototypes
            .Sum(task => task.Fabrics.Where(f => f.DeterminedByOrder).Sum(f => f.Quantity));

        this.DecorMaterialQuantity = this.TaskPrototypes
            .Sum(task => task.Materials.Where(m => m.DeterminedByOrder).Sum(m => m.Quantity));
    }

    public int CalculateResourcePrice()
    {
        return this.TaskPrototypes
            .Sum(task => task.CalculateResourcePrice());
    }

    public int CalculatePayment()
    {
        return this.TaskPrototypes
            .Sum(task => task.Payment);
    }
}

public class DesignPhoto : Photo
{
    public int DesignId { get; set; }
}
