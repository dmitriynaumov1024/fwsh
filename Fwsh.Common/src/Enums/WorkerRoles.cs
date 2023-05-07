namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class WorkerRoles
{
    public static readonly string 
        Unknown = "unknown",
        Management = "management",
        Sewing = "sewing",
        Carpentry = "carpentry",
        Assembly = "assembly",
        Upholstery = "upholstery";

    public static readonly ICollection<string> KnownWorkerRoles = new List<string>
    {
        Sewing, Carpentry, Assembly, Upholstery
    };

    public static readonly ICollection<string> KnownValues = new List<string> 
    {
        Unknown, Management, Sewing, Carpentry, Assembly, Upholstery
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
