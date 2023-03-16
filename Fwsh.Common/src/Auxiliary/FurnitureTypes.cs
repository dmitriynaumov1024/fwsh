namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class FurnitureTypes
{
    public static readonly string 
        Unknown = "unknown",
        Sofa = "sofa",
        Corner = "corner",
        Ottoman = "ottoman",
        Armchair = "armchair",
        Pouffe = "pouffe";

    public static readonly List<string> KnownValues = new List<string> 
    {
        Unknown, Sofa, Corner, Ottoman, Armchair, Pouffe
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
