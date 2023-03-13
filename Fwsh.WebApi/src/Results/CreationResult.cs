namespace Fwsh.WebApi.Results;

public class CreationResult : SuccessResult
{
    public object Id { get; set; }

    public CreationResult () { }

    public CreationResult (object id, string message = null) 
    {
        this.Id = id;
        this.Message = message;
    }
}
