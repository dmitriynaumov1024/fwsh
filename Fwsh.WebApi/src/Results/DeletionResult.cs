namespace Fwsh.WebApi.Results;

public class DeletionResult : Result
{
    public object Id { get; set; }
    public string Message { get; set; }

    public DeletionResult () { }

    public DeletionResult (object id, string message = null) 
    {
        this.Id = id;
        this.Message = message;
    }
}
