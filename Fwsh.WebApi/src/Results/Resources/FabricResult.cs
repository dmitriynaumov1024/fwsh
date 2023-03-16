namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;
using Fwsh.Common;
using Fwsh.WebApi.Results.Catalog;

public class FabricResult : ResourceResult
{
    public string MeasureUnit { get; set; }
    public string PhotoUrl { get; set; }

    public ColorResult Color { get; set; }
    public FabricTypeResult FabricType { get; set; }

    public FabricResult () { }

    public FabricResult (Fabric fabric)
    {
        this.Id = fabric.Id;
        this.Name = fabric.Name;
        this.Description = fabric.Description;
        this.PhotoUrl = fabric.PhotoUrl;
        this.PricePerUnit = fabric.PricePerUnit;
        this.MeasureUnit = fabric.MeasureUnit;
        this.CreatedAt = fabric.CreatedAt;

        this.Color = new ColorResult(fabric.Color);
        this.FabricType = new FabricTypeResult(fabric.FabricType);
    }
}
