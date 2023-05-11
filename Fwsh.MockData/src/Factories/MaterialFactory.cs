namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class MaterialFactory : Factory<Resource>
{
    static Resource[] data = new[] {
        new Resource {
            Name = "Foam 50mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 45
        },
        new Resource {
            Name = "Foam 25mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 24
        },
        new Resource {
            Name = "Foam 80mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 80
        },
        new Resource {
            Name = "Wood",
            MeasureUnit = MeasureUnits.CubicMeters,
            Precision = 5,
            PricePerUnit = 4000
        },
        new Resource {
            Name = "Wooden slab",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 60
        },
        new Resource {
            Name = "Plywood 3-layer",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 40
        },
        new Resource {
            Name = "PVA glue",
            MeasureUnit = MeasureUnits.Liters,
            Precision = 3,
            PricePerUnit = 90
        },
        new Resource {
            Name = "Foam glue",
            MeasureUnit = MeasureUnits.Liters,
            Precision = 3,
            PricePerUnit = 140
        },
        new Resource {
            Name = "Synthepone",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 20
        },
        new Resource {
            Name = "Interlining fabric",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 15
        },
        new Resource {
            Color = new Color {
                Name = "Oak wood",
                RgbCode = "#c4c399"
            },
            Name = "Oak wood decorative slab",
            SlotName = SlotNames.Decor,
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 84
        },
        new Resource {
            Color = new Color {
                Name = "Dark walnut wood",
                RgbCode = "#695050"
            },
            Name = "Dark walnut decorative slab",
            SlotName = SlotNames.Decor,
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 89
        },
        new Resource {
            Color = new Color {
                Name = "Birch wood",
                RgbCode = "#f0f3e0"
            },
            Name = "Birch decorative slab",
            SlotName = SlotNames.Decor,
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 85
        },
        new Resource {
            Color = new Color {
                Name = "Alder wood",
                RgbCode = "#ecb889"
            },
            Name = "Alder decorative slab",
            SlotName = SlotNames.Decor,
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 78
        }
    };

    public override int? FixedSize => data.Length;

    public override Resource Next() => data[0];

    public override Resource[] All() => data.ToArray();
}
