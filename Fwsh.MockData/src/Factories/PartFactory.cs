namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class PartFactory : Factory<StoredResource>
{
    static StoredResource[] data = new[] {
        new StoredResource {
            Name = "Bolt 6x40",
            PricePerUnit = 0.4
        },
        new StoredResource {
            Name = "Screw 55mm",
            PricePerUnit = 0.2
        },
        new StoredResource {
            Name = "Screw 75mm",
            PricePerUnit = 0.3
        },
        new StoredResource {
            Name = "Basic hinge",
            PricePerUnit = 22
        },
        new StoredResource {
            Name = "Spring mechanism",
            PricePerUnit = 45
        },
        new StoredResource {
            Name = "Hinge mechanism",
            PricePerUnit = 80
        },
        new StoredResource {
            Name = "Swing mechanism",
            PricePerUnit = 60
        },
        new StoredResource {
            Name = "Plastic leg",
            PricePerUnit = 4
        },
        new StoredResource {
            Name = "Metal tube joint",
            PricePerUnit = 5
        }
    };

    public override int? FixedSize => data.Length;

    public override StoredResource Next() => data[0];

    public override StoredResource[] All() => data.ToArray();
}
