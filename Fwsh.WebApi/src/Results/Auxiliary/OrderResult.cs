namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using Fwsh.Common;

public class OrderResult
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Status { get; set; }
    public int Price { get; set; }
    public int Payment { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime? ReceivedAt { get; set; }

    public CustomerResult Customer { get; set; }
    public List<NotificationResult> Notifications { get; set; }

    public OrderResult() { }

    public OrderResult (Order order)
    {
        this.Id = order.Id;
        this.CustomerId = order.CustomerId;
        this.Status = order.Status;
        this.CreatedAt = order.CreatedAt;
        this.StartedAt = order.StartedAt;
        this.FinishedAt = order.FinishedAt;
        this.ReceivedAt = order.ReceivedAt;
    }
}
