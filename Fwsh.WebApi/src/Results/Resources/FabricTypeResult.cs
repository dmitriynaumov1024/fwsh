namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;

using Fwsh.Common;

public class FabricTypeResult
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } 

    public FabricTypeResult (FabricType fabricType) 
    {
        this.Id = fabricType.Id;
        this.Name = fabricType.Name;
        this.Description = fabricType.Description;
        this.CreatedAt = fabricType.CreatedAt;
    }
}
