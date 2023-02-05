namespace Fwsh.Common;

public class ProductionOrderPaymentEvent : BasicEvent
{
    public int OrderId { get; set; }
    public int Balance { get; set; }
}
