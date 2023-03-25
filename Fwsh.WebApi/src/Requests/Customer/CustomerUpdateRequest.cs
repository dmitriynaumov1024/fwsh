namespace Fwsh.WebApi.Requests.Customer;

using Fwsh.Common;
using Fwsh.Utils;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class CustomerUpdateRequest : Request, UpdateRequest<Customer>
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("newPassword", this.NewPassword)
                .NotNull().LengthInRange(8, 64);
    }

    public void ApplyTo (Customer customer)
    {
        customer.Password = this.NewPassword.QuickHash();
    }
}
