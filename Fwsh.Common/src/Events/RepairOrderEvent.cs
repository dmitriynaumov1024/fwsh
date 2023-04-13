namespace Fwsh.Common;

public class RepairOrderEvent : BasicEvent
{
    public int OrderId { get; set; }

    public RepairOrderEvent (RepairOrder order): base()
    {
        this.OrderId = order.Id;
    }
}
