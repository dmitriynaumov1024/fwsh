namespace Fwsh.WebApi.Results.Catalog;

using System;
using System.Collections.Generic;
using Fwsh.Common;

public class FabricResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public double PricePerUnit { get; set; }
    public string MeasureUnit { get; set; }
    public DateTime CreatedAt { get; set; }
    public int DaysSinceCreated { get; set; }

    public ColorResult Color { get; set; }
    public FabricTypeResult FabricType { get; set; }

    public FabricResult (Fabric fabric)
    {
        this.Id = fabric.Id;
        this.Name = fabric.Name;
        this.Description = fabric.Description;
        this.PhotoUrl = fabric.PhotoUrl;
        this.PricePerUnit = fabric.PricePerUnit;
        this.MeasureUnit = fabric.MeasureUnit;
        this.CreatedAt = fabric.CreatedAt;
        this.DaysSinceCreated = (DateTime.UtcNow - this.CreatedAt).Days;

        this.Color = new ColorResult(fabric.Color);
        this.FabricType = new FabricTypeResult(fabric.FabricType);
    }
}
