namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class FabricTypeFactory : Factory<FabricType>
{
    static FabricType[] data = new[] {
        new FabricType {
            Name = "Amsterdam",
            Description = "Careless city patterns with variable background color"
        },
        new FabricType {
            Name = "Seashore velvet",
            Description = "Soft and fluffy but durable fabric that resembles sea breeze"
        },
        new FabricType {
            Name = "Eco leather",
            Description = "Eco leather is already a classic thing with a wide range of colors to match the any room interior"
        }
    };

    public override int? FixedSize => data.Length;

    public override FabricType Next() => data[0];

    public override FabricType[] All() => data.ToArray();
}
