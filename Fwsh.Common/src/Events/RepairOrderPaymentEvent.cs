namespace Fwsh.Common;

public class RepairOrderPaymentEvent : BasicEvent
{
    public int OrderId { get; set; }
    public int Balance { get; set; }
}
