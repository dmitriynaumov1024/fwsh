namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;
using Fwsh.Database;

public class FwshDataSeeder
{
    Random random = new Random();

    Factory<Tuple<string, string, string>> FullName = new FullNameFactory();
    Factory<string> PhoneNumber = new PhoneNumberFactory();
    Factory<string> Password = new PasswordFactory();
    Factory<string> Email = new EmailFactory(); 
    Factory<string> OrgName = new OrgNameFactory();
    Factory<ICollection<WorkerRole>> WorkerRoles = new WorkerRoleFactory();
    Factory<Color> Color = new ColorFactory();
    Factory<FabricType> FabricType = new FabricTypeFactory();

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
                Password = this.Password.Next()
            };
            worker.Roles = this.WorkerRoles.Next();
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
            context.Customers.Add (
                new Customer {
                    Surname = surname,
                    Name = name,
                    Patronym = patronym,
                    Phone = this.PhoneNumber.Next(),
                    Email = this.Email.Next(),
                    Password = this.Password.Next()
                }
            );
        }
    }

    void SeedColors (FwshDataContext context)
    {
        context.Colors.AddRange (this.Color.All());
    }

    void SeedFabricTypes (FwshDataContext context)
    {
        context.FabricTypes.AddRange (this.FabricType.All());
    }

    public void Seed (FwshDataContext context)
    {
        this.SeedWorkers(context, 14);
        this.SeedSuppliers(context, 6);
        this.SeedCustomers(context, 25);

        this.SeedColors(context);
        this.SeedFabricTypes(context);
    }
}
