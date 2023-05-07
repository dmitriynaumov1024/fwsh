namespace Fwsh.Common;

using System;

public class Color 
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string RgbCode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
