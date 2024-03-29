namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class TaskStatus
{
    public static readonly string
        Unknown = "unknown",
        Assigned = "assigned",
        Rejected = "rejected",
        Working = "working",
        Finished = "finished",
        Paid = "paid",
        Impossible = "impossible";

    public static readonly ICollection<string> KnownValues = new List<string> 
    {
        Unknown, Assigned, Rejected, Working, Finished, Paid, Impossible
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
