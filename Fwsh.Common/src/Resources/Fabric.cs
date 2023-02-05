namespace Fwsh.Common;

public class Fabric : Resource 
{
    public int ColorId { get; set; }
    public int FabricTypeId { get; set; }

    public string PhotoUrl { get; set; }

    public string MeasureUnit => MeasureUnits.SquareMeters;

    public virtual Color Color { get; set; }
    public virtual FabricType FabricType { get; set; }
}
