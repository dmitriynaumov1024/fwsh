namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public abstract class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }

    public int Price { get; set; }
    public int Payment { get; set; }

    public string Status { get; set; } = OrderStatus.Unknown;
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime? ReceivedAt { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual bool TrySetStatus (string status)
    {
        if (! OrderStatus.Contains(status)) return false;

        this.Status = status;

        if (status == OrderStatus.Working) 
            this.StartedAt ??= DateTime.UtcNow;

        if (status == OrderStatus.Finished) 
            this.FinishedAt ??= DateTime.UtcNow;

        if (status == OrderStatus.Received) 
            this.ReceivedAt ??= DateTime.UtcNow;

        return true;
    }
}
