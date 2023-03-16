namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;
using Fwsh.Common;
using Fwsh.WebApi.Results.Catalog;

public class MaterialResult : ResourceResult
{
    public bool IsDecorative { get; set; }
    public string MeasureUnit { get; set; }
    public string PhotoUrl { get; set; }

    public ColorResult Color { get; set; }

    public MaterialResult () { }

    public MaterialResult (Material mat)
    {
        this.Id = mat.Id;
        this.Name = mat.Name;
        this.Description = mat.Description;
        this.PricePerUnit = mat.PricePerUnit;
        this.CreatedAt = mat.CreatedAt;
        this.IsDecorative = mat.IsDecorative;
        this.MeasureUnit = mat.MeasureUnit;
        this.PhotoUrl = mat.PhotoUrl;

        this.Color = new ColorResult(mat.Color);
    }
}