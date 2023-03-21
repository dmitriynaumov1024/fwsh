namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class FabricType
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int DaysSinceCreated => (DateTime.UtcNow - this.CreatedAt).Days;
}
