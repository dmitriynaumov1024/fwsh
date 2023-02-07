namespace Fwsh.Common;

using System;

public abstract class Resource  
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public double PricePerUnit { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
