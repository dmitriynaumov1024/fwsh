namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class SlotNames
{
    public static readonly string
        Unknown = null,
        Fabric = "fabric",
        Decor = "decor";

    public static readonly ICollection<string> KnownValues = new List<string> 
    {
        Unknown, Fabric, Decor
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
