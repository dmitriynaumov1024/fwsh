namespace Fwsh.WebApi.Results;

public class DeletionResult : SuccessResult
{
    public object Id { get; set; }

    public DeletionResult () { }

    public DeletionResult (object id, string message = null) 
    {
        this.Id = id;
        this.Message = message;
    }
}
