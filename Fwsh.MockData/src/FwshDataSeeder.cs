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

    public void Seed (FwshDataContext context)
    {
        this.SeedCustomers(context, 20);
    }
}
