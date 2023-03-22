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

    // Object-like entities --------------------------------------------------

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Worker> Workers { get; set; }
    // Worker paychecks, Worker roles are accessible via Worker
    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Design> Designs { get; set; }
    public DbSet<TaskPrototype> TaskPrototypes { get; set; }
    // TaskPart, TaskMaterial, TaskFabric are accessible via TaskPrototype

    public DbSet<Color> Colors { get; set; }
    public DbSet<FabricType> FabricTypes { get; set; }

    public DbSet<Part> Parts { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Fabric> Fabrics { get; set; }
    
    public DbSet<StoredPart> StoredParts { get; set; }
    public DbSet<StoredMaterial> StoredMaterials { get; set; }
    public DbSet<StoredFabric> StoredFabrics { get; set; }

    public DbSet<PartSupplyOrder> PartSupplyOrders { get; set; }
    public DbSet<MaterialSupplyOrder> MaterialSupplyOrders { get; set; }
    public DbSet<FabricSupplyOrder> FabricSupplyOrders { get; set; }

    public DbSet<ProductionOrder> ProductionOrders { get; set; }
    public DbSet<RepairOrder> RepairOrders { get; set; }

    public DbSet<ProductionNotification> ProductionNotifications { get; set; }
    public DbSet<RepairNotification> RepairNotifications { get; set; }

    public DbSet<ProductionTask> ProductionTasks { get; set; }
    public DbSet<RepairTask> RepairTasks { get; set; }

    // Events ----------------------------------------------------------------

    public DbSet<SignupEvent> SignupEvents { get; set; }

    public DbSet<ProductionOrderEvent> ProductionOrderEvents { get; set; }
    public DbSet<RepairOrderEvent> RepairOrderEvents { get; set; }

    public DbSet<ProductionEvent> ProductionEvents { get; set; }
    public DbSet<RepairEvent> RepairEvents { get; set; }

    public DbSet<ProductionPaymentEvent> ProductionPaymentEvents { get; set; }
    public DbSet<RepairPaymentEvent> RepairPaymentEvents { get; set; }

    public DbSet<ResourcePurchaseEvent> ResourcePurchaseEvents { get; set; }
    public DbSet<ResourceUsageEvent> ResourceUsageEvents { get; set; }

    public DbSet<PaycheckReceiveEvent> PaycheckReceiveEvents { get; set; }


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
            entity.HasKey("Id");

            entity.HasIndex("NameKey")
                .IsUnique();

            entity.Ignore("DimCompact");
            entity.Ignore("DimExpanded");
        });

        modelBuilder.Entity<TaskPrototype>(entity => {
            entity.HasMany(task => task.Parts)
                .WithOne()
                .HasForeignKey("TaskId");
            
            entity.HasMany(task => task.Materials)
                .WithOne()
                .HasForeignKey("TaskId");

            entity.HasMany(task => task.Fabrics)
                .WithOne()
                .HasForeignKey("TaskId");
        });

        modelBuilder.Entity<TaskPart>(entity => {
            entity.HasKey("Id");

            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("PartId");
        });

        modelBuilder.Entity<TaskMaterial>(entity => {
            entity.HasKey("Id");
            
            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("MaterialId")
                .IsRequired(false);
        });

        modelBuilder.Entity<TaskFabric>(entity => {
            entity.HasKey("Id");
            
            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("FabricId")
                .IsRequired(false);
        });

        modelBuilder.Entity<StoredPart>(entity => {
            entity.HasKey("Id");

            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("Id");
        });

        modelBuilder.Entity<StoredMaterial>(entity => {
            entity.HasKey("Id");

            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("Id");
        });

        modelBuilder.Entity<StoredFabric>(entity => {
            entity.HasKey("Id");

            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("Id");
        });

        modelBuilder.Entity<Customer>(customer => {
            customer.HasIndex("Phone")
                .IsUnique();
        });

        modelBuilder.Entity<Worker>(worker => {
            worker.HasIndex("Phone")
                .IsUnique();

            worker.HasMany(w => w.Roles)
                .WithOne()
                .HasForeignKey("WorkerId");
                
            worker.HasMany(w => w.Paychecks)
                .WithOne()
                .HasForeignKey("WorkerId");
        });

        modelBuilder.Entity<WorkerRole>(entity => {
            entity.HasKey("WorkerId", "RoleName");
        });
    }
}
