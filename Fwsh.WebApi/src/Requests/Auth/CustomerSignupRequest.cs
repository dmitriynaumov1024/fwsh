namespace Fwsh.WebApi.Requests.Auth;

using System;
using System.Text.RegularExpressions;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class CustomerSignupRequest : Request, CreationRequest<Customer>
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public bool IsOrganization { get; set; }
    public string OrgName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("surname", this.Surname)
                .NotNull().LengthInRange(2, 30);

        validator.Property("name", this.Name)
                .NotNull().LengthInRange(2, 24);

        validator.Property("patronym", this.Patronym)
                .NotNull().LengthInRange(4, 28);

        if (this.IsOrganization) {
            validator.Property("orgName", this.OrgName)
                .NotNull().LengthInRange(2, 32);
        }

        validator.Property("phone", this.Phone)
                .NotNull().Match(PhoneRegex);

        validator.Property("email", this.Email)
                .NotNull().Match(EmailRegex);

        validator.Property("password", this.Password)
                .NotNull().LengthInRange(8, 64);
    }

    public Customer Create ()
    {
        return new Customer() {
            Surname = this.Surname,
            Name = this.Name,
            Patronym = this.Patronym,
            OrgName = this.OrgName,
            IsOrganization = this.IsOrganization,
            Phone = this.Phone,
            Email = this.Email,
            Password = this.Password.QuickHash()
        };
    }

    static Regex PhoneRegex = new Regex(@"^\+?[0-9]{10,14}$");
    static Regex EmailRegex = new Regex(@"^[a-z0-9-]{2,64}@([a-z0-9-]{1,60}.){1,5}[a-z0-9]{2,10}$");
}
