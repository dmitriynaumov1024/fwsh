namespace Fwsh.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Fwsh.Common;

public class FwshDataContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Design> Designs { get; set; }
    
    public DbSet<Part> Parts { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Fabric> Fabrics { get; set; }
    
    public DbSet<StoredPart> StoredParts { get; set; }
    public DbSet<StoredMaterial> StoredMaterials { get; set; }
    public DbSet<StoredFabric> StoredFabrics { get; set; }

    public DbSet<ProductionOrder> ProductionOrders { get; set; }
    public DbSet<RepairOrder> RepairOrders { get; set; }

    public DbSet<ProductionTask> ProductionTasks { get; set; }
    public DbSet<RepairTask> RepairTasks { get; set; }

    // TO Be DONE
    // Events are not yet in the graph.

    public FwshDataContext() { }

    public FwshDataContext(DbContextOptions<FwshDataContext> options) : base(options) { }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSnakeCaseNamingConvention()
            .UseNpgsql ( String.Format (
                "Host={0};Port={1};Database={2};Username={3};Password={4}",
                "192.168.0.104", 5432, "fwshtest", "postgres", "postgres20"
            ));
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
            entity.HasKey("TaskId", "PartId");

            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("PartId");
        });

        modelBuilder.Entity<TaskMaterial>(entity => {
            entity.HasKey("TaskId", "MaterialId");

            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("MaterialId");
        });

        modelBuilder.Entity<TaskFabric>(entity => {
            entity.HasKey("TaskId", "FabricId");

            entity.HasOne(t => t.Item)
                .WithMany()
                .HasForeignKey("FabricId");
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
