namespace Fwsh.WebApi.Results.Catalog;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;

public class MiniDesignResult
{
    public int Id { get; set; }
    public string NameKey { get; set; }
    public string Type { get; set; }
    public string DisplayName { get; set; }
    public int Price { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<string> PhotoUrls { get; set; }

    public MiniDesignResult (Design design, bool includePhotos = true)
    {
        this.Id = design.Id;
        this.NameKey = design.NameKey;
        this.Type = design.Type;
        this.DisplayName = design.DisplayName;
        this.Price = design.Price;
        this.CreatedAt = design.CreatedAt;
        
        if (includePhotos) this.PhotoUrls = new List<string> { 
            design.Photos.MinBy(p => p.Position)?.Url 
        };
    }
}
