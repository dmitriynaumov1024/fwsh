namespace Fwsh.WebApi.Results;

public class FailResult : Result
{
    public bool Success => false;

    public string Message { get; set; }

    public FailResult () { }

    public FailResult (string message = null) 
    {
        this.Message = message;
    }
}
