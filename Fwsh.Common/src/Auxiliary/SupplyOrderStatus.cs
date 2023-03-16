namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class SupplyOrderStatus
{
    public static readonly string
        Unknown = "unknown",
        Submitted = "submitted",
        ReceivedAndPaid = "receivedpaid",
        Impossible = "impossible";

    public static readonly List<string> KnownValues = new List<string> 
    {
        Unknown, Submitted, ReceivedAndPaid, Impossible
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
