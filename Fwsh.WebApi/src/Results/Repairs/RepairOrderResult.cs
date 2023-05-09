namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class RepairOrderResult : OrderResult, IResultBuilder<RepairOrderResult>
{
    private RepairOrder order;

    public List<string> PhotoUrls { get; set; }
    public string Description { get; set; }

    public RepairOrderResult() { }

    public RepairOrderResult (RepairOrder order)
    {
        this.order = order;
    }

    public RepairOrderResult Mini ()
    {
        return new RepairOrderResult() {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            StartedAt = order.StartedAt,
            FinishedAt = order.FinishedAt,
            ReceivedAt = order.ReceivedAt,
            Price = order.Price,
            Payment = order.Payment,
            PhotoUrls = order.PhotoUrls
        };
    }

    public RepairOrderResult ForCustomer ()
    {
        var result = Mini();
        result.Description = order.Description;
        result.Notifications = order.Notifications
            .OrderBy(n => n.Id)
            .Select(n => new NotificationResult(n))
            .ToList();

        return result;
    }

    public RepairOrderResult ForWorker ()
    {
        var result = ForCustomer();
        return result;
    }

    public RepairOrderResult ForManager ()
    {
        var result = ForWorker();
        result.Customer ??= new CustomerResult(order.Customer);
        return result;
    }
}
