namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class DesignResult : Result, IResultBuilder<DesignResult>
{
    private Design design { get; set; }

    public int Id { get; set; }
    public string NameKey { get; set; }
    public string Type { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool IsVisible { get; set; }

    public bool IsTransformable { get; set; }
    public Dimensions DimCompact { get; set; }
    public Dimensions DimExpanded { get; set; }

    public double FabricQuantity { get; set; }
    public double DecorMaterialQuantity { get; set; }

    public int Price { get; set; }

    public DateTime CreatedAt { get; set; }
    public int DaysSinceCreated => (DateTime.UtcNow - this.CreatedAt).Days;

    public DateTime? PriceRecalculatedAt { get; set; }

    // TaskPrototypes { get; set; }
    public List<string> PhotoUrls { get; set; }

    public DesignResult () { }

    public DesignResult (Design design)
    {
        this.design = design;
    }

    public DesignResult Mini ()
    {
        return new DesignResult() {
            Id = design.Id,
            NameKey = design.NameKey,
            Type = design.Type,
            DisplayName = design.DisplayName,
            IsVisible = design.IsVisible,
            Price = design.Price,
            CreatedAt = design.CreatedAt,
            PhotoUrls = new List<string> {
                design.Photos.MinBy(p => p.Position)?.Url
            }
        };
    }

    public DesignResult ForCustomer () 
    {
        return new DesignResult() {
            Id = design.Id,
            NameKey = design.NameKey,
            Type = design.Type,
            DisplayName = design.DisplayName,
            Price = design.Price,
            CreatedAt = design.CreatedAt,
            Description = design.Description,
            IsVisible = design.IsVisible,
            IsTransformable = design.IsTransformable,
            DimCompact = design.DimCompact,
            DimExpanded = design.DimExpanded,
            PhotoUrls = design.Photos
                .OrderBy(p => p.Position)
                .Select(p => p.Url).ToList()
        };
    }

    public DesignResult ForWorker () 
    {
        var result = this.ForCustomer();
        result.FabricQuantity = design.FabricQuantity;
        result.DecorMaterialQuantity = design.DecorMaterialQuantity;
        return result;
    }

    public DesignResult ForManager ()
    {
        return this.ForWorker();
    }
}
