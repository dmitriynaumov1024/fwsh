namespace Fwsh.WebApi.Results.Customer;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Results.Common;
using Fwsh.WebApi.Results.Customer;

public class RepairOrderResult : MiniRepairOrderResult
{
    public string Description { get; set; }
    public CustomerProfileResult Customer { get; set; }
    public ICollection<NotificationResult> Notifications { get; set; }

    public RepairOrderResult (RepairOrder order) : base(order)
    {
        this.Description = order.Description;
        this.Customer = new CustomerProfileResult(order.Customer);
        this.Notifications = order.Notifications
            .Select(n => new NotificationResult(n)).ToList();
    }
}
