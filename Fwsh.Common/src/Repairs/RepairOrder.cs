namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class RepairOrder : Order
{
    public string Description { get; set; }

    public virtual List<string> PhotoUrls { get; set; }
    public virtual ICollection<RepairNotification> Notifications { get; set; }
    public virtual ICollection<RepairTask> Tasks { get; set; }
    
    public RepairOrder() : base()
    {
        this.PhotoUrls = new List<string>();
        this.Notifications = new HashSet<RepairNotification>();
        this.Tasks = new HashSet<RepairTask>();
    }
}
