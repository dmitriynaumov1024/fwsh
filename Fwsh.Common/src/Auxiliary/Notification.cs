namespace Fwsh.Common;

using System;

public abstract class Notification
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
