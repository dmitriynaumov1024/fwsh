namespace Fwsh.Common;

public class ProductionOrderEvent : BasicEvent
{
    public int DesignId { get; set; }
    public int OrderId { get; set; }
    public int FabricId { get; set; }
    public int FabricColorId { get; set; }
    public int FabricTypeId { get; set; }
    public int Quantity { get; set; }

    public ProductionOrderEvent(ProductionOrder order) : base()
    {
        this.OrderId = order.Id;
        this.DesignId = order.DesignId;
        this.FabricId = order.FabricId;
        this.FabricColorId = order.Fabric.ColorId;
        this.FabricTypeId = order.Fabric.FabricTypeId;
        this.Quantity = order.Quantity;
    }
}
