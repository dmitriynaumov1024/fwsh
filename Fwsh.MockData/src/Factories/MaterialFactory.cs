namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class MaterialFactory : Factory<StoredResource>
{
    static StoredResource[] data = new[] {
        new StoredResource {
            Name = "Foam 50mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 45
        },
        new StoredResource {
            Name = "Foam 25mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 24
        },
        new StoredResource {
            Name = "Foam 80mm",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 80
        },
        new StoredResource {
            Name = "Wood",
            MeasureUnit = MeasureUnits.CubicMeters,
            Precision = 5,
            PricePerUnit = 4000
        },
        new StoredResource {
            Name = "Wooden slab",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 60
        },
        new StoredResource {
            Name = "Plywood 3-layer",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 40
        },
        new StoredResource {
            Name = "PVA glue",
            MeasureUnit = MeasureUnits.Liters,
            Precision = 3,
            PricePerUnit = 90
        },
        new StoredResource {
            Name = "Foam glue",
            MeasureUnit = MeasureUnits.Liters,
            Precision = 3,
            PricePerUnit = 140
        },
        new StoredResource {
            Name = "Synthepone",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 20
        },
        new StoredResource {
            Name = "Interlining fabric",
            MeasureUnit = MeasureUnits.SquareMeters,
            Precision = 1,
            PricePerUnit = 15
        },
        new StoredResource {
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
        new StoredResource {
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
        new StoredResource {
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
        new StoredResource {
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

    public override StoredResource Next() => data[0];

    public override StoredResource[] All() => data.ToArray();
}
