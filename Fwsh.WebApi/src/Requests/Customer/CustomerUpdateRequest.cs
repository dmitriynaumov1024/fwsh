namespace Fwsh.WebApi.Requests.Customer;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class CustomerUpdateRequest : Request
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.DoNothing();
    }
}
