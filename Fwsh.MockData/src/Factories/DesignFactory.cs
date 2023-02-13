namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class DesignFactory : Factory<Design>
{
    Random random = new Random();

    static Design[] data = new[] {
        new Design {
            NameKey = "prague",
            DisplayName = "Prague",
            Description = "Compact transformable couch",
            IsTransformable = true,
            DimCompact = new Dimensions {
                Length = 100,
                Width = 170,
                Height = 90
            },
            DimExpanded = new Dimensions {
                Length = 190,
                Width = 170,
                Height = 90
            }
        },
        new Design {
            NameKey = "lyra",
            DisplayName = "Lyra",
            Description = "Compact transformable couch",
            IsTransformable = true,
            DimCompact = new Dimensions {
                Length = 105,
                Width = 165,
                Height = 60
            },
            DimExpanded = new Dimensions {
                Length = 210,
                Width = 165,
                Height = 60
            }
        },
        new Design {
            NameKey = "barras",
            DisplayName = "Barras",
            Description = "Transformable corner couch",
            IsTransformable = true,
            DimCompact = new Dimensions {
                Length = 140,
                Width = 170,
                Height = 90
            },
            DimExpanded = new Dimensions {
                Length = 210,
                Width = 170,
                Height = 90
            }
        },
        new Design {
            NameKey = "londonese",
            DisplayName = "Londonese",
            Description = "Comfortable armchair",
            IsTransformable = false,
            DimCompact = new Dimensions {
                Length = 95,
                Width = 85,
                Height = 92
            },
            DimExpanded = new Dimensions {
                Length = 95,
                Width = 85,
                Height = 92
            }
        },
        new Design {
            NameKey = "chester",
            DisplayName = "Chester",
            Description = "Compact round pouffe",
            IsTransformable = false,
            DimCompact = new Dimensions {
                Length = 45,
                Width = 45,
                Height = 48
            }
        }
    };

    public override int? FixedSize => data.Length;

    public override Design Next() => random.Choice(data);

    public override Design[] All() => data.ToArray();
}
