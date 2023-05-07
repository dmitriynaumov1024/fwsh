namespace Fwsh.Common;

using System;

public class WorkerPaycheck
{
    public int Id { get; set; }
    public int WorkerId { get; set; }

    public DateTime IntervalStart { get; set; }
    public DateTime IntervalEnd { get; set; }
    
    public int Amount { get; set; }

    public bool IsReceived { get; set; }

    public WorkerPaycheck()
    {
    }
}
