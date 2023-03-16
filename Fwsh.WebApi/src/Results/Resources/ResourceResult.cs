namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;
using Fwsh.Common;
using Fwsh.WebApi.Results;

public class ResourceResult : Result
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double PricePerUnit { get; set; }
    public DateTime CreatedAt { get; set; }

    public ResourceResult () { }
}
