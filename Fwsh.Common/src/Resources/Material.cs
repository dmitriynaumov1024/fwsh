namespace Fwsh.Common;

using System;

public class Material : Resource 
{
    public string MeasureUnit { get; set; }

    public int? ColorId { get; set; }
    public string PhotoUrl { get; set; }

    public virtual Color Color { get; set; }
}
