namespace Fwsh.WebApi.Requests.Auth;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class PasswordUpdateRequest : Request, UpdateRequest<Person>
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("newPassword", this.NewPassword)
                .NotNull().LengthInRange(8, 64);

        validator.DoNothing();
    }

    public bool PasswordMatch (Person person)
    {
        return this.OldPassword.QuickHash() == person.Password;
    }

    public void ApplyTo (Person person)
    {
        person.Password = this.NewPassword.QuickHash();
    }
}
