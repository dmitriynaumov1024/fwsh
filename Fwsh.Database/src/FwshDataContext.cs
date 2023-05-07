namespace Fwsh.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Fwsh.Common;

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
        if (this.Configure != null) {
            this.Configure(optionsBuilder);
        }
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Design>(entity => {
            entity.HasKey(d => d.Id);

            entity.HasIndex(d => d.NameKey)
                .IsUnique();

            entity.Property(d => d.DimCompact)
                .HasConversion<DimensionsConverter>();

            entity.Property(d => d.DimExpanded)
                .HasConversion<DimensionsConverter>();
        });

        modelBuilder.Entity<Customer>(customer => {
            customer.HasIndex(c => c.Phone)
                .IsUnique();
        });

        modelBuilder.Entity<Worker>(worker => {
            worker.HasIndex(c => c.Phone)
                .IsUnique();

            worker.Property(w => w.Roles)
                .HasConversion<StringListConverter>();
        });
    }
}
