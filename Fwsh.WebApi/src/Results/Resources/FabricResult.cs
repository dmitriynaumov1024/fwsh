namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;
using Fwsh.Common;

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

        if (fabric.Color != null) {
            this.Color = new ColorResult(fabric.Color);
        }
        
        if (fabric.FabricType != null) {
            this.FabricType = new FabricTypeResult(fabric.FabricType);
        }
    }
}
