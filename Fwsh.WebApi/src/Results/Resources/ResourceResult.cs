namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class ResourceResult : Result
{
    public int Id { get; set; }
    public int? ColorId { get; set; }
    public int? FabricTypeId { get; set; }

    public string Type { get; set; }
    public string SlotName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int Precision { get; set; } = 0; 
    public string MeasureUnit { get; set; }
    public double PricePerUnit { get; set; }

    public string PhotoUrl { get; set; }

    public virtual ColorResult Color { get; set; }
    public virtual FabricTypeResult FabricType { get; set; }

    public DateTime CreatedAt { get; set; }

    public int DaysSinceCreated => (DateTime.UtcNow - this.CreatedAt).Days;

    public ResourceResult () { }

    public ResourceResult (Resource res) 
    { 
        this.Id = res.Id;
        this.ColorId = res.ColorId;
        this.FabricTypeId = res.FabricTypeId;
        this.Type = res.Type;
        this.SlotName = res.SlotName;
        this.Name = res.Name;
        this.Description = res.Description;
        this.Precision = res.Precision;
        this.MeasureUnit = res.MeasureUnit;
        this.PricePerUnit = res.PricePerUnit;
        this.PhotoUrl = res.PhotoUrl;
        this.CreatedAt = res.CreatedAt;

        if (res.Color != null) 
            this.Color = new ColorResult(res.Color);

        if (res.FabricType != null)
            this.FabricType = new FabricTypeResult(res.FabricType);
    }
}
