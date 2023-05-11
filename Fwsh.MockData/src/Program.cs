namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;

public class Program
{
    public static void Main (string[] args) 
    {
        Logger credentialsLogger = null;
        if (args.Contains("log-credentials")) {
            credentialsLogger = new ConsoleLogger();
        }

        FwshDataContext context = new FwshDataContextPostgres();
        
        Seeder[] seeders = new Seeder[] {
            new PeopleSeeder() { CredentialsLogger = credentialsLogger },
            new ResourceSeeder(),
            new BlueprintSeeder(),
            new ProductionSeeder(),
            new RepairSeeder()
        };

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (args.Contains("seed")) {
            Console.WriteLine("\n -- SEEDING DATABASE -- \n");
            foreach (var seeder in seeders) seeder.Seed(context);
            context.SaveChanges();
        }

        Console.WriteLine(" -- Done. Shutting down -- \n");
    }
}
