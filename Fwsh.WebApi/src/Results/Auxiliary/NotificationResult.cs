namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class NotificationResult
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public NotificationResult() { }

    public NotificationResult (Notification n) 
    {
        this.Id = n.Id;
        this.Text = n.Text;
        this.IsRead = n.IsRead;
        this.CreatedAt = n.CreatedAt;
    }
}
