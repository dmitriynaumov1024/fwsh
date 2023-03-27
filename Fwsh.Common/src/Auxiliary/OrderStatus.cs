namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class OrderStatus
{
    public static readonly string
        Unknown = "unknown",
        Submitted = "submitted",
        Delayed = "delayed",
        Working = "working",
        Finished = "finished",
        ReceivedAndPaid = "receivedpaid",
        Rejected = "rejected",
        Impossible = "impossible";

    public static readonly List<string> KnownValues = new List<string> 
    {
        Unknown, Submitted, Delayed, Working, Finished, 
        ReceivedAndPaid, Rejected, Impossible
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
