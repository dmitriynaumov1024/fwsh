namespace Fwsh.WebApi.Results;

public class CreatedResult
{
    public object Id { get; set; }
    public string Message { get; set; }

    public CreatedResult (object id, string message = null) 
    {
        this.Id = id;
        this.Message = message;
    }
}
