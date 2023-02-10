namespace Fwsh.Common;

public class ResourceUsageEvent : BasicEvent
{
    public string ResourceType { get; set; }
    public int WorkerId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}
