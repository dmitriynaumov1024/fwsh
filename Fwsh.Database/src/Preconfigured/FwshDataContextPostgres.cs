namespace Fwsh.Database;

using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using Fwsh.Utils; 

public class FwshDataContextPostgres : FwshDataContext
{
    public FwshDataContextPostgres() : base(null) { }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        #warning FwshDataContextPostgres relies on DB_HOST, DB_PORT, DB_DATABASE, DB_USER, DB_PASSWORD variables from either environment or .env file in working directory. An Exception will be thrown if one or more variables are not found. 

        base.OnConfiguring(optionsBuilder);

        string[] requiredEnv = new[] { 
            "DB_HOST", "DB_PORT", "DB_DATABASE", "DB_USER", "DB_PASSWORD" 
        };

        var missingEnv = requiredEnv.Where(key => env.get(key) == null).ToList(); 
        
        if (missingEnv.Count > 0) {
            throw new Exception (
                "One or more environment variables were not found: " +
                String.Join(", ", missingEnv)
            );
        }

        optionsBuilder.UseSnakeCaseNamingConvention();

        optionsBuilder.UseNpgsql ( String.Format (
            "Host={0};Port={1};Database={2};Username={3};Password={4}",
            env.get("DB_HOST"), env.get("DB_PORT"), env.get("DB_DATABASE"), 
            env.get("DB_USER"), env.get("DB_PASSWORD")
        ));
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
