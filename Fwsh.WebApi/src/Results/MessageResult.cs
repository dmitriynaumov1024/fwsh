namespace Fwsh.WebApi.Results;

public class MessageResult
{
    public string Message { get; set; }

    public MessageResult (string message) 
    {
        this.Message = message;
    }
}