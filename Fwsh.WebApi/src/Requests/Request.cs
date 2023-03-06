namespace Fwsh.WebApi.Requests;

using Fwsh.WebApi.Results;

public abstract class Request
{
    public abstract Result Validate();

    public RequestValidator Validator() 
    {
        return new RequestValidator();
    }
}
