namespace Fwsh.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Fwsh.Common;
using Fwsh.Logging;
using Fwsh.Utils;

public class FwshDataContext : DbContext
{
    private Action<DbContextOptionsBuilder> Configure;

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Design> Designs { get; set; }
    public DbSet<TaskPrototype> TaskPrototypes { get; set; }

    public DbSet<Color> Colors { get; set; }
    public DbSet<FabricType> FabricTypes { get; set; }

    public DbSet<StoredResource> StoredResources { get; set; }
    public DbSet<SupplyOrder> SupplyOrders { get; set; }

    public DbSet<ProdOrder> ProdOrders { get; set; }
    public DbSet<ProdFurniture> ProdFurniture { get; set; }
    public DbSet<ProdTask> ProdTasks { get; set; }
    public DbSet<ProdNotification> ProdNotifications { get; set; }

    public DbSet<RepairOrder> RepairOrders { get; set; }
    public DbSet<RepairNotification> RepairNotifications { get; set; }
    public DbSet<RepairTask> RepairTasks { get; set; }


    public FwshDataContext(Action<DbContextOptionsBuilder> configure = null) : base()
    { 
        this.Configure = configure;
    }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        if (this.Configure != null) this.Configure(optionsBuilder);

        this.ConfigureLogging(optionsBuilder);
    }

    void ConfigureLogging (DbContextOptionsBuilder optionsBuilder)
    {
        var loggingCategories = new List<string>();
        if (env.isTrue("DB_LOG_QUERIES")) loggingCategories.Add(DbLoggerCategory.Query.Name);
        if (env.isTrue("DB_LOG_UPDATES")) loggingCategories.Add(DbLoggerCategory.Update.Name); 

        if (loggingCategories.Count > 0) {
            Logger logger = env.get("DB_LOGFILE")?.ToLower() switch {
                null => new ConsoleLogger(),
                "console" => new ConsoleLogger(),
                string filename => FileLogger.To(filename)
            };

            if (env.isDevelopment) optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(message => logger.Log(message));
        }
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Design>(design => {
            design.HasKey(d => d.Id);
            design.HasIndex(d => d.NameKey).IsUnique();
            design.Property(d => d.DimCompact).HasConversion<DimensionsConverter>();
            design.Property(d => d.DimExpanded).HasConversion<DimensionsConverter>();
            design.Property(d => d.PhotoUrls).HasConversion<StringListConverter>();
        });

        modelBuilder.Entity<TaskPrototype>(task => {
            task.Ignore(t => t.Parts);
            task.Ignore(t => t.Materials);
            task.Ignore(t => t.Fabrics);
        });

        modelBuilder.Entity<Customer>(customer => {
            customer.HasIndex(c => c.Phone).IsUnique();
        });

        modelBuilder.Entity<Worker>(worker => {
            worker.HasIndex(w => w.Phone).IsUnique();
            worker.Ignore(w => w.Roles);
        });

        modelBuilder.Entity<RepairOrder>(order => {
            order.Property(o => o.PhotoUrls).HasConversion<StringListConverter>();
        });
    }
}
