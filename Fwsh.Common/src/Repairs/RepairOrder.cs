namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class RepairOrder : Order
{
    public string Description { get; set; }

    public int Price { get; set; }
    public int Prepayment { get; set; }

    public virtual ICollection<RepairOrderPhoto> Photos { get; set; }
    public virtual ICollection<RepairNotification> Notifications { get; set; }
    public virtual ICollection<RepairTask> Tasks { get; set; }
    
    public RepairOrder() : base()
    {
        this.Notifications = new HashSet<RepairNotification>();
        this.Photos = new HashSet<RepairOrderPhoto>();
        this.Tasks = new HashSet<RepairTask>();
    }
}

public class RepairOrderPhoto : Photo 
{
    public int OrderId { get; set; }
}
