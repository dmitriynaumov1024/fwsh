namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public abstract class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    
    public string Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime? ReceivedAt { get; set; }
}
