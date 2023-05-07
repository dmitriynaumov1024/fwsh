namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class ResourceTypes
{
    public static readonly string 
        Resource = "resource",
        Part = "part",
        Material = "material",
        Fabric = "fabric";

    public static readonly ICollection<string> KnownValues = new List<string> 
    {
        Resource, Part, Material, Fabric
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
} 
