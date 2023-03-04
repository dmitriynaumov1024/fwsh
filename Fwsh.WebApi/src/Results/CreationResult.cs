namespace Fwsh.WebApi.Results;

public class CreationResult
{
    public object Id { get; set; }
    public string Message { get; set; }

    public CreationResult () { }

    public CreationResult (object id, string message = null) 
    {
        this.Id = id;
        this.Message = message;
    }
}
