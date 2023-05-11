namespace Fwsh.MockData;

using System;

using Fwsh.Database;

public abstract class Seeder
{
    public abstract void Seed (FwshDataContext context);
}
