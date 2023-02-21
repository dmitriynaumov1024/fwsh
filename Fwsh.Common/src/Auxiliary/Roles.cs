namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class Roles
{
    public static readonly string 
        Unknown = "unknown",
        Customer = "customer",
        Management = "management",
        Carpentry = "carpentry",
        Sewing = "sewing",
        Assembly = "assembly",
        Upholstery = "upholstery";

    public static readonly ICollection<string> KnownWorkerRoles =
    new HashSet<string> {
        Roles.Carpentry,
        Roles.Sewing,
        Roles.Assembly,
        Roles.Upholstery
    };
}
