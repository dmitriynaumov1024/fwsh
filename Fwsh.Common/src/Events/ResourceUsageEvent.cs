namespace Fwsh.Common;

public class ResourceUsageEvent : BasicEvent
{
    public int WorkerId { get; set; }

    public string TaskType { get; set; }
    public int TaskId { get; set; }

    public string ResourceType { get; set; }
    public int ItemId { get; set; }

    public int Quantity { get; set; }
}
