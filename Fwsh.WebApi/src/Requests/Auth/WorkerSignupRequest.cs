namespace Fwsh.WebApi.Requests.Auth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class WorkerSignupRequest : Request, CreationRequest<Worker>
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; } = new List<string>();

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("surname", this.Surname)
                .NotNull().LengthInRange(2, 30);

        validator.Property("name", this.Name)
                .NotNull().LengthInRange(2, 24);

        validator.Property("patronym", this.Patronym)
                .NotNull().LengthInRange(4, 28);

        validator.Property("phone", this.Phone)
                .NotNull().Match(PhoneRegex);

        validator.Property("email", this.Email)
                .NotNull().Match(EmailRegex);

        validator.Property("password", this.Password)
                .NotNull().LengthInRange(8, 64);

        validator.Property("roles", this.Roles)
                .NotNull().CountInRange(0, 4);
    }

    public Worker Create ()
    {
        return new Worker() {
            Surname = this.Surname,
            Name = this.Name,
            Patronym = this.Patronym,
            Phone = this.Phone,
            Email = this.Email,
            Password = this.Password.QuickHash(),
            Roles = new HashSet<string>(this.Roles)
                .Where(WorkerRoles.KnownWorkerRoles.Contains).ToList()
        };
    }

    static Regex PhoneRegex = new Regex(@"^\+?[0-9]{10,14}$");
    static Regex EmailRegex = new Regex(@"^[a-z0-9-]{2,64}@([a-z0-9-]{1,60}.){1,5}[a-z0-9]{2,10}$");
}
