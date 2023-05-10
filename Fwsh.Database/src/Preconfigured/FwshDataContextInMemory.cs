namespace Fwsh.Database;

using System;
using Microsoft.EntityFrameworkCore;

using Fwsh.Common;
using Fwsh.Utils;

public class FwshDataContextInMemory : FwshDataContext
{
    public FwshDataContextInMemory() : base(null) { }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseInMemoryDatabase(env.get("DB_DATABASE"));
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
