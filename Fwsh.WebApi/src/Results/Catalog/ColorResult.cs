namespace Fwsh.WebApi.Results.Catalog;

using System;
using System.Collections.Generic;
using Fwsh.Common;

public class ColorResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string RgbCode { get; set; }

    public ColorResult (Color color)
    {
        if (color == null) return;

        this.Id = color.Id;
        this.Name = color.Name;
        this.RgbCode = color.RgbCode;
    }
}
