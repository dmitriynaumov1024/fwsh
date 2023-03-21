namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class ResourceResult : Result
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double PricePerUnit { get; set; }
    public DateTime CreatedAt { get; set; }

    public int DaysSinceCreated => (DateTime.UtcNow - this.CreatedAt).Days;

    public ResourceResult () { }
}
