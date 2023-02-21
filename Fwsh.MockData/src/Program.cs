namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;

public class Program
{
    public static void Main (string[] args) 
    {
        Logger credentialsLogger = null;
        if (args.Length >= 1 && args[0] == "verbose") {
            credentialsLogger = new ConsoleLogger();
        } 

        FwshDataContext context = new FwshDataContextPostgres();
        FwshDataSeeder seeder = new FwshDataSeeder() {
            CredentialsLogger = credentialsLogger
        };

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("\n -- SEEDING DATABASE -- \n");
        
        seeder.Seed(context);
        context.SaveChanges();

        Console.WriteLine(" -- Done. Shutting down -- \n");
    }
}
