namespace Fwsh.Common;

public class ProductionPaymentEvent : BasicEvent
{
    public int OrderId { get; set; }
    public int Balance { get; set; }
}
