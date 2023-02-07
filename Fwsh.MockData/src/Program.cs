namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;
using Fwsh.Database;

public class Program
{
    public static void Main (string[] args) 
    {
        FwshDataContext context = new FwshDataContext();
        FwshDataSeeder seeder = new FwshDataSeeder();
        context.Database.EnsureCreated();
        seeder.Seed(context);
        context.SaveChanges();
    }
}
