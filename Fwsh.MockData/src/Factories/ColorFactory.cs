namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class ColorFactory : Factory<Color>
{
    static Color[] data = new[] {
        new Color {
            Name = "Fuchsia",
            RgbCode = "#991687"
        },
        new Color {
            Name = "Beige",
            RgbCode = "#fee8e7"
        },
        new Color {
            Name = "Pearl White",
            RgbCode = "#f9f8f7"
        },
        new Color {
            Name = "Ash Gray",
            RgbCode = "#cecece"
        },
        new Color {
            Name = "Clean Black",
            RgbCode = "#121113"
        },
        new Color {
            Name = "Chocolate",
            RgbCode = "#7f5659"
        }, 
        new Color {
            Name = "Grass Green",
            RgbCode = "#5fc04f"
        },
        new Color {
            Name = "Sunny Yellow",
            RgbCode = "#f7f087"
        },
        new Color {
            Name = "Sand",
            RgbCode = "#f9e6ab"
        }
    };

    public override int? FixedSize => data.Length;

    public override Color Next() => data[0];

    public override Color[] All() => data.ToArray();
}
