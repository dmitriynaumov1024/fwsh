namespace Fwsh.WebApi.Results;

public class SuccessResult : Result
{
    public bool Success => true;
    
    public string Message { get; set; }

    public SuccessResult () { }

    public SuccessResult (string message = null) 
    {
        this.Message = message;
    }
}
