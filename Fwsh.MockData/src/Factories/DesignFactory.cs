namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class DesignFactory : Factory<Design>
{
    Random random = new Random();

    static string cm = MeasureUnits.Centimeters;

    static Design[] data = new[] {
        new Design {
            NameKey = "prague",
            DisplayName = "Prague",
            Type = FurnitureTypes.Ottoman,
            Description = "Compact transformable ottoman couch",
            IsVisible = true,
            IsTransformable = true,
            DimCompact = new Dimensions (170, 100, 90, cm),
            DimExpanded = new Dimensions (170, 190, 90, cm)
        },
        new Design {
            NameKey = "lyra",
            DisplayName = "Lyra",
            Type = FurnitureTypes.Ottoman,
            Description = "Compact transformable couch",
            IsVisible = true,
            IsTransformable = true,
            DimCompact = new Dimensions (165, 105, 60, cm),
            DimExpanded = new Dimensions (165, 201, 60, cm)
        },
        new Design {
            NameKey = "barras",
            DisplayName = "Barras",
            Type = FurnitureTypes.Corner,
            Description = "Transformable corner couch",
            IsVisible = true,
            IsTransformable = true,
            DimCompact = new Dimensions (170, 140, 90, cm),
            DimExpanded = new Dimensions (170, 210, 90, cm)
        },
        new Design {
            NameKey = "londonese",
            DisplayName = "Londonese",
            Type = FurnitureTypes.Armchair,
            Description = "Comfortable armchair",
            IsVisible = true,
            IsTransformable = false,
            DimCompact = new Dimensions (85, 95, 92, cm)
        },
        new Design {
            NameKey = "chester",
            DisplayName = "Chester",
            Type = FurnitureTypes.Pouffe,
            Description = "Compact round pouffe",
            IsVisible = true,
            IsTransformable = false,
            DimCompact = new Dimensions (45, 45, 48, cm)
        }
    };

    public override int? FixedSize => data.Length;

    public override Design Next() => random.Choice(data);

    public override Design[] All() => data.ToArray();
}
