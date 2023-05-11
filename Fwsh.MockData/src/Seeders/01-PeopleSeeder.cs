namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.Utils;

public class PeopleSeeder : Seeder
{ 
    public Logger CredentialsLogger { get; set; }

    Random random = new Random();

    Factory<Tuple<string, string, string>> FullName = new FullNameFactory();
    Factory<string> PhoneNumber = new PhoneNumberFactory();
    Factory<string> Password = new PasswordFactory();
    Factory<string> Email = new EmailFactory(); 
    Factory<List<string>> Roles = new WorkerRoleFactory();
    Factory<string> OrgName = new OrgNameFactory();

    void SeedManagers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            var manager = new Worker {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next(),
                Password = this.Password.Next(),
                Roles = new[] { WorkerRoles.Management }
            };
            this.CredentialsLogger?.Log("manager {{\n  \"phone\": \"{0}\",\n  \"password\": \"{1}\"\n}}",
                                        manager.Phone, manager.Password);
            manager.Password = manager.Password.QuickHash();
            context.Workers.Add(manager);
        }
    }

    void SeedWorkers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            var worker = new Worker {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next(),
                Password = this.Password.Next(),
                Roles = this.Roles.Next()
            };
            this.CredentialsLogger?.Log("worker {{\n  \"phone\": \"{0}\",\n  \"password\": \"{1}\"\n}}",
                                        worker.Phone, worker.Password);
            worker.Password = worker.Password.QuickHash();
            context.Workers.Add(worker);
        }
    }

    void SeedSuppliers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            var supplier = new Supplier {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                OrgName = this.OrgName.Next(),
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next()
            };
            context.Suppliers.Add(supplier);
        }
    }

    void SeedCustomers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            var customer = new Customer {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next(),
                Password = this.Password.Next()
            };
            this.CredentialsLogger?.Log("customer {{\n  \"phone\": \"{0}\",\n  \"password\": \"{1}\"\n}}",
                                        customer.Phone, customer.Password);
            customer.Password = customer.Password.QuickHash();
            context.Customers.Add(customer);
        }
    }


    public override void Seed (FwshDataContext context)
    {
        this.SeedManagers(context, 2);
        this.SeedWorkers(context, 14);
        this.SeedSuppliers(context, 6);
        this.SeedCustomers(context, 25);
    }
}
