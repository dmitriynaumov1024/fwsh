namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class MaterialFactory : Factory<Material>
{
    static Material[] data = new[] {
        new Material {
            Name = "Foam 50mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            PricePerUnit = 45
        },
        new Material {
            Name = "Foam 25mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            PricePerUnit = 24
        },
        new Material {
            Name = "Foam 80mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            PricePerUnit = 80
        },
        new Material {
            Name = "Wood",
            MeasureUnit = MeasureUnits.CubicMeters,
            PricePerUnit = 4000
        },
        new Material {
            Name = "Wooden slab",
            MeasureUnit = MeasureUnits.SquareMeters,
            PricePerUnit = 60
        },
        new Material {
            Name = "Plywood 3-layer",
            MeasureUnit = MeasureUnits.SquareMeters,
            PricePerUnit = 40
        },
        new Material {
            Name = "PVA glue",
            MeasureUnit = MeasureUnits.Liters,
            PricePerUnit = 90
        },
        new Material {
            Name = "Foam glue",
            MeasureUnit = MeasureUnits.Liters,
            PricePerUnit = 140
        },
        new Material {
            Name = "Synthepone",
            MeasureUnit = MeasureUnits.SquareMeters,
            PricePerUnit = 20
        },
        new Material {
            Name = "Interlining fabric",
            MeasureUnit = MeasureUnits.SquareMeters,
            PricePerUnit = 15
        }
    };

    public override int? FixedSize => data.Length;

    public override Material Next() => data[0];

    public override Material[] All() => data.ToArray();
}
