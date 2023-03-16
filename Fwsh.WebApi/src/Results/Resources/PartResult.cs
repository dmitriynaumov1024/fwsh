namespace Fwsh.WebApi.Results.Resources;

using System;
using Fwsh.Common;

public class PartResult : ResourceResult
{
    public PartResult () { }

    public PartResult (Part part)
    {
        this.Id = part.Id;
        this.Name = part.Name;
        this.Description = part.Description;
        this.PricePerUnit = part.PricePerUnit;
        this.CreatedAt = part.CreatedAt;
    }
}
