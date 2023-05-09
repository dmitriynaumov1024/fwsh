namespace Fwsh.WebApi.Requests.Manager;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class PersonUpdateRequest : Request, UpdateRequest<Person>
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public bool IsOrganization { get; set; }
    public string OrgName { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; }

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

        validator.Property("password", this.Password)
                .LengthInRange(8, 64);
    }

    public void ApplyTo (Person person)
    {
        person.Surname = this.Surname;
        person.Name = this.Name;
        person.Patronym = this.Patronym;
       
        if (this.Password != null) {
            person.Password = this.Password.QuickHash();
        }
        
        if (person is Customer customer) {
            customer.IsOrganization = this.IsOrganization;
            customer.OrgName = this.OrgName;
        }

        if (person is Worker worker) {
            worker.Roles = new HashSet<string>(this.Roles)
                .Where(WorkerRoles.KnownWorkerRoles.Contains).ToList();
        }
       
    }
}
