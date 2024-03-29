namespace Fwsh.WebApi.Requests.Customer;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class CustomerUpdateRequest : Request, UpdateRequest<Customer>
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public bool IsOrganization { get; set; }
    public string OrgName { get; set; }
    public string OldPassword { get; set; }
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

        validator.Property("oldPassword", this.OldPassword)
                .NotNull();

        validator.Property("password", this.Password)
                .NotNull().LengthInRange(8, 64);
    }

    public void ApplyTo (Customer customer)
    {
        customer.Surname = this.Surname;
        customer.Name = this.Name;
        customer.Patronym = this.Patronym;
        customer.IsOrganization |= this.IsOrganization;
        if (this.OrgName != null) customer.OrgName = this.OrgName;
        if (this.Password != null) customer.Password = this.Password.QuickHash();
    }
}
