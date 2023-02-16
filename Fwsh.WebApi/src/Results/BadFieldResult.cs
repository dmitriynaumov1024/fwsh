namespace Fwsh.WebApi.Results;

public class BadFieldResult
{
    public string[] BadFields { get; set; }

    public BadFieldResult (params string[] fields)
    {
        this.BadFields = fields;
    }
}
