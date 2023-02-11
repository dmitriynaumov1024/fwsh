namespace Fwsh.Database;

using System;
using Microsoft.EntityFrameworkCore;

public class FwshDataContextInMemory : FwshDataContext
{
    public FwshDataContextInMemory() : base(null) { }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseInMemoryDatabase("fwsh");
    }
}
