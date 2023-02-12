namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class PartFactory : Factory<Part>
{
    static Part[] data = new[] {
        new Part {
            Name = "Bolt 6x40",
            PricePerUnit = 0.4
        },
        new Part {
            Name = "Screw 55mm",
            PricePerUnit = 0.2
        },
        new Part {
            Name = "Screw 75mm",
            PricePerUnit = 0.3
        },
        new Part {
            Name = "Basic hinge",
            PricePerUnit = 22
        },
        new Part {
            Name = "Spring mechanism",
            PricePerUnit = 45
        },
        new Part {
            Name = "Hinge mechanism",
            PricePerUnit = 80
        },
        new Part {
            Name = "Swing mechanism",
            PricePerUnit = 60
        },
        new Part {
            Name = "Plastic leg",
            PricePerUnit = 4
        },
        new Part {
            Name = "Metal tube joint",
            PricePerUnit = 5
        }
    };

    public override int? FixedSize => data.Length;

    public override Part Next() => data[0];

    public override Part[] All() => data.ToArray();
}
