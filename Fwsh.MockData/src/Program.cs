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
        FwshDataContext context = new FwshDataContextPostgres();
        FwshDataSeeder seeder = new FwshDataSeeder();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        Console.WriteLine("\n -- SEEDING DATABASE -- \n");
        seeder.Seed(context);
        context.SaveChanges();
        Console.WriteLine(" -- Done. Shutting down -- \n");
    }
}
