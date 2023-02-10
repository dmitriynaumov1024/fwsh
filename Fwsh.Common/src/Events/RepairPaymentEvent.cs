namespace Fwsh.Common;

public class RepairPaymentEvent : BasicEvent
{
    public int OrderId { get; set; }
    public int Balance { get; set; }
}
