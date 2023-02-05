namespace Fwsh.Common;

public class RepairEvent : BasicEvent
{
    public int OrderId { get; set; }
    public string Status { get; set; }
}
