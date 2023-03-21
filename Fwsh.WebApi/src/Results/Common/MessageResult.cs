namespace Fwsh.WebApi.Results;

public class MessageResult : Result
{
    public string Message { get; set; }

    public MessageResult () { }

    public MessageResult (string message = null) 
    {
        this.Message = message;
    }
}
