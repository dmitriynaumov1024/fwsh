namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class PartFactory : Factory<Resource>
{
    static Resource[] data = new[] {
        new Resource {
            Name = "Bolt 6x40",
            PricePerUnit = 0.4
        },
        new Resource {
            Name = "Screw 55mm",
            PricePerUnit = 0.2
        },
        new Resource {
            Name = "Screw 75mm",
            PricePerUnit = 0.3
        },
        new Resource {
            Name = "Basic hinge",
            PricePerUnit = 22
        },
        new Resource {
            Name = "Spring mechanism",
            PricePerUnit = 45
        },
        new Resource {
            Name = "Hinge mechanism",
            PricePerUnit = 80
        },
        new Resource {
            Name = "Swing mechanism",
            PricePerUnit = 60
        },
        new Resource {
            Name = "Plastic leg",
            PricePerUnit = 4
        },
        new Resource {
            Name = "Metal tube joint",
            PricePerUnit = 5
        }
    };

    public override int? FixedSize => data.Length;

    public override Resource Next() => data[0];

    public override Resource[] All() => data.ToArray();
}
