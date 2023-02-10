namespace Fwsh.Common;

public class ResourcePurchaseEvent : BasicEvent
{
    public string ResourceType { get; set; }
    public int ItemId { get; set; }
    public int SupplierId { get; set; }
    public int Quantity { get; set; }
    public int Balance { get; set; }
}
