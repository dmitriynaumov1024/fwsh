namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class FurnitureTypes
{
    public static readonly string 
        Unknown = "unknown",
        Sofa = "sofa",
        Minisofa = "minisofa",
        Ottoman = "ottoman",
        Corner = "corner",
        Armchair = "armchair",
        Pouffe = "pouffe";

    public static readonly ICollection<string> KnownValues = new List<string> 
    {
        Unknown, Sofa, Minisofa, Ottoman, Corner, Armchair, Pouffe
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
