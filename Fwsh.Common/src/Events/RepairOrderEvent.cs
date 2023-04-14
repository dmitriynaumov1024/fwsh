namespace Fwsh.Common;

public class RepairOrderEvent : BasicEvent
{
    public int OrderId { get; set; }

    public RepairOrderEvent() : base() { }

    public RepairOrderEvent (RepairOrder order): base()
    {
        this.OrderId = order.Id;
    }
}
