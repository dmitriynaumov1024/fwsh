namespace Fwsh.WebApi.Requests.Auth;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;

public class LoginRequest : Request
{
    public string Phone { get; set; }
    public string Password { get; set; }

    public override Result Validate()
    {
        return (Result) true;
    }
}
