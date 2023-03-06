namespace Fwsh.WebApi.Results.Catalog;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class DesignResult : MiniDesignResult
{
    public string Description { get; set; }

    public bool IsTransformable { get; set; }
    public Dimensions DimCompact { get; set; }
    public Dimensions DimExpanded { get; set; }

    public DesignResult (Design design) : base(design, false)
    {
        this.Description = design.Description;
        this.IsTransformable = design.IsTransformable;
        this.DimCompact = design.DimCompact;
        this.DimExpanded = design.DimExpanded;

        this.PhotoUrls = design.Photos
            .OrderBy(p => p.Position)
            .Select(p => p.Url).ToList();
    }
}
