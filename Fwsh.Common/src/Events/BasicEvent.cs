namespace Fwsh.Common;

using System;

public abstract class BasicEvent
{
    public int Id { get; set; }
    
    public int EventYear { get; set; } = DateTime.UtcNow.Year;
    public int EventMonth { get; set; } = DateTime.UtcNow.Month;
    public int EventDay { get; set; } = DateTime.UtcNow.Day;
}
