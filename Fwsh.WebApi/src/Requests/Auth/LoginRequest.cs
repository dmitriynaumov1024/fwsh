namespace Fwsh.WebApi.Requests.Auth;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class LoginRequest : Request
{
    public string Phone { get; set; }
    public string Password { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.DoNothing();
    }
}
