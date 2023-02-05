namespace Fwsh.Common;

public class ProductionOrderEvent : BasicEvent
{
    public int DesignId { get; set; }
    public int OrderId { get; set; }
    public int FabricId { get; set; }
    public int FabricColorId { get; set; }
    public int FabricTypeId { get; set; }
    public int Quantity { get; set; }
}
