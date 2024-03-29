namespace Fwsh.WebApi.Requests.Worker;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class WorkerUpdateRequest : Request, UpdateRequest<Worker>
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public string Password { get; set; }
    public string OldPassword { get; set; }
    public List<string> Roles { get; set; } = new List<string>();

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
                
        validator.Property("roles", this.Roles)
                .NotNull().CountInRange(0, 4);
    }

    public void ApplyTo (Worker worker)
    {
        worker.Surname = this.Surname;
        worker.Name = this.Name;
        worker.Patronym = this.Patronym;
        worker.Password = this.Password.QuickHash();

        worker.Roles = new HashSet<string>(this.Roles)
            .Where(WorkerRoles.KnownWorkerRoles.Contains).ToList();
    }
}
