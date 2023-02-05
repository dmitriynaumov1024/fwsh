namespace Fwsh.Common;

public class ProductionEvent : BasicEvent
{
    public int DesignId { get; set; }
    public int OrderId { get; set; }
    public string Status { get; set; }
}
