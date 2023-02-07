namespace Fwsh.Common;

public static class OrderStatus
{
    public static readonly string
        Unknown = "unknown",
        Submitted = "submitted",
        Delayed = "delayed",
        Production = "production",
        Finished = "finished",
        ReceivedAndPaid = "receivedpaid",
        Rejected = "rejected",
        Impossible = "impossible";
}
