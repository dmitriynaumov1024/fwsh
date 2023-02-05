namespace Fwsh.Common;

public class PaycheckReceiveEvent : BasicEvent
{
    public int PaycheckId { get; set; }
    public int Balance { get; set; }
}
