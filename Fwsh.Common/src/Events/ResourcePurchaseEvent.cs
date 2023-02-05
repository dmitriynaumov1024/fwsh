namespace Fwsh.Common;

public abstract class ResourcePurchaseEvent : BasicEvent
{
    public int SupplierId { get; set; }
    public int Balance { get; set; }
    public int Quantity { get; set; }
}
