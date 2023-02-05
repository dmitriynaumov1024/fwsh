namespace Fwsh.Common;

using System;

public class WorkerPaycheck
{
    public int Id { get; set; }
    public int WorkerId { get; set; }

    public int Amount { get; set; }

    public DateTime IntervalStart { get; set; }
    public DateTime IntervalEnd { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReceivedAt { get; set; }

    public WorkerPaycheck()
    {
    }
}
