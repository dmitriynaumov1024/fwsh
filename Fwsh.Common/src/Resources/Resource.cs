namespace Fwsh.Common;

using System;

public class Resource  
{
    public int Id { get; set; }
    public int? ColorId { get; set; }
    public int? FabricTypeId { get; set; }

    public string Type { get; set; } = ResourceTypes.Resource;
    public string SlotName { get; set; } = SlotNames.Unknown;
    public string Name { get; set; }
    public string Description { get; set; }

    public int Precision { get; set; } = 0; 
    public string MeasureUnit { get; set; } = MeasureUnits.Unknown;
    public double PricePerUnit { get; set; }

    public string PhotoUrl { get; set; }

    public virtual Color Color { get; set; }
    public virtual FabricType FabricType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int DaysSinceCreated => (DateTime.UtcNow - this.CreatedAt).Days;
}
