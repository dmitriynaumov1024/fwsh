namespace Fwsh.WebApi.Results.Catalog;

using System;
using System.Collections.Generic;
using Fwsh.Common;

public class DecorMaterialResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MeasureUnit { get; set; }
    public int PricePerUnit { get; set; }
    public string PhotoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public int DaysSinceCreated { get; set; }

    public ColorResult Color { get; set; }

    public DecorMaterialResult (Material m) 
    {
        if (m == null) return;

        this.Id = m.Id;
        this.Name = m.Name;
        this.MeasureUnit = m.MeasureUnit;
        this.PricePerUnit = (int)m.PricePerUnit;
        this.PhotoUrl = m.PhotoUrl;
        this.CreatedAt = m.CreatedAt;
        this.DaysSinceCreated = (DateTime.UtcNow - this.CreatedAt).Days;

        this.Color = new ColorResult(m.Color);
    }
}
