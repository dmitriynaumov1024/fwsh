namespace Fwsh.WebApi.Requests.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using Fwsh.Utils;

public class PersonRequest : Request, UpdateRequest<Person>
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public bool IsOrganization { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
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

        validator.Property("phone", this.Phone)
                .NotNull().Match(PhoneRegex);

        validator.Property("email", this.Email)
                .NotNull().Match(EmailRegex);

        if (this.IsOrganization) {
            validator.Property("orgName", this.OrgName)
                .NotNull().LengthInRange(2, 32);
        }
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

        if (person is Supplier supplier) {
            supplier.OrgName = this.OrgName;
        }

        if (person is Worker worker) {
            worker.Roles = new HashSet<string>(this.Roles)
                .Where(WorkerRoles.KnownWorkerRoles.Contains).ToList();
        }

        if (person.Phone == null || person is Supplier) {
            person.Phone = this.Phone;
            person.Email = this.Email;
        } 
    }

    static Regex PhoneRegex = new Regex(@"^\+?[0-9]{10,14}$");
    static Regex EmailRegex = new Regex(@"^[a-z0-9-]{2,64}@([a-z0-9-]{1,60}.){1,5}[a-z0-9]{2,10}$");
}
