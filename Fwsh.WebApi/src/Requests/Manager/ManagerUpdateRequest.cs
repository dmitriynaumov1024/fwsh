namespace Fwsh.WebApi.Requests.Manager;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class ManagerUpdateRequest : Request, UpdateRequest<Worker>
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public string Password { get; set; }
    public string OldPassword { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("surname", this.Surname)
                .NotNull().LengthInRange(2, 30);

        validator.Property("name", this.Name)
                .NotNull().LengthInRange(2, 24);

        validator.Property("patronym", this.Patronym)
                .NotNull().LengthInRange(4, 28);

        validator.Property("password", this.Password)
                .NotNull().LengthInRange(8, 64);

        validator.Property("oldPassword", this.OldPassword)
                .NotNull();
    }

    public void ApplyTo (Worker worker)
    {
        worker.Surname = this.Surname;
        worker.Name = this.Name;
        worker.Patronym = this.Patronym;
        worker.Password = this.Password.QuickHash();
    }
}
