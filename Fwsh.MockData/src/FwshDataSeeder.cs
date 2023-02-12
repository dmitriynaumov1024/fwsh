namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;
using Fwsh.Database;

public class FwshDataSeeder
{
    Random random = new Random();

    Factory<Tuple<string, string, string>> FullName = new FullNameFactory();
    Factory<string> PhoneNumber = new PhoneNumberFactory();
    Factory<string> Password = new PasswordFactory();
    Factory<string> Email = new EmailFactory(); 
    Factory<string> OrgName = new OrgNameFactory();
    Factory<string> ExternalId = new ExternalIdFactory();
    Factory<ICollection<WorkerRole>> WorkerRoles = new WorkerRoleFactory();
    Factory<Color> Color = new ColorFactory();
    Factory<FabricType> FabricType = new FabricTypeFactory();
    Factory<Material> Materials = new MaterialFactory();
    Factory<Part> Parts = new PartFactory();

    void SeedWorkers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            var worker = new Worker {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next(),
                Password = this.Password.Next()
            };
            worker.Roles = this.WorkerRoles.Next();
            context.Workers.Add(worker);
        }
    }

    void SeedSuppliers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            var supplier = new Supplier {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                OrgName = this.OrgName.Next(),
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next()
            };
            context.Suppliers.Add(supplier);
        }
    }

    void SeedCustomers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            context.Customers.Add (
                new Customer {
                    Surname = surname,
                    Name = name,
                    Patronym = patronym,
                    Phone = this.PhoneNumber.Next(),
                    Email = this.Email.Next(),
                    Password = this.Password.Next()
                }
            );
        }
    }

    void SeedColors (FwshDataContext context)
    {
        context.Colors.AddRange (this.Color.All());
    }

    void SeedFabricTypes (FwshDataContext context)
    {
        context.FabricTypes.AddRange (this.FabricType.All());
    }

    void SeedFabrics (FwshDataContext context)
    {
        var fabrics = context.Colors.Local
            .Join ( context.FabricTypes.Local, 
                color => !color.Name.Contains("wood"), fabric => true, 
                (color, fabric) => new Tuple<Color, FabricType>(color, fabric) 
            )
            .Where(_ => random.Probability(0.85))
            .ToList();

        foreach (var (color, fabric) in fabrics) {
            context.Fabrics.Add ( new Fabric {
                Name = $"{fabric.Name} {color.Name}",
                Description = fabric.Description,
                PricePerUnit = random.Next(150, 290),
                PhotoUrl = "/stub.jpg",
                Color = color,
                FabricType = fabric
            });
        }
    }

    void SeedMaterials (FwshDataContext context)
    {
        context.Materials.AddRange(this.Materials.All()); 

        var woodColors = context.Colors.Local
            .Where(color => color.Name.Contains("wood"))
            .ToList();

        foreach (var color in woodColors) {
            context.Materials.Add ( new Material {
                Name = $"{color.Name} decorative slab",
                MeasureUnit = MeasureUnits.SquareMeters,
                IsDecorative = true,
                PricePerUnit = 80,
                PhotoUrl = "/stub.jpg",
                Color = color
            });
        }
    }

    void SeedParts (FwshDataContext context)
    {
        context.Parts.AddRange(this.Parts.All());
    }

    void SeedStoredFabrics (FwshDataContext context)
    {
        var fabrics = context.Fabrics.Local.ToList();
        var suppliers = context.Suppliers.Local.ToArray();

        foreach (var fabric in fabrics) {
            context.StoredFabrics.Add ( new StoredFabric {
                Item = fabric,
                Supplier = random.Choice(suppliers),
                ExternalId = this.ExternalId.Next(),
                Quantity = random.Next(60, 98),
                NormalStock = 100,
                RefillPeriodDays = 30
            });
        }
    }

    void SeedStoredMaterials (FwshDataContext context)
    {
        var materials = context.Materials.Local.ToList();
        var suppliers = context.Suppliers.Local.ToArray();

        foreach (var mat in materials) {
            context.StoredMaterials.Add ( new StoredMaterial {
                Item = mat,
                Supplier = random.Choice(suppliers),
                ExternalId = this.ExternalId.Next(),
                Quantity = (int)((double)random.Next(4400, 5000) / Math.Sqrt(mat.PricePerUnit)),
                NormalStock = (int)((double)random.Next(4950, 6000) / Math.Sqrt(mat.PricePerUnit)),
                RefillPeriodDays = 14
            });
        }
    }

    void SeedStoredParts (FwshDataContext context)
    {
        var parts = context.Parts.Local.ToList();
        var suppliers = context.Suppliers.Local.ToArray();

        foreach (var part in parts) {
            context.StoredParts.Add ( new StoredPart {
                Item = part,
                Supplier = random.Choice(suppliers),
                ExternalId = this.ExternalId.Next(),
                Quantity = (int)((double)random.Next(3600, 4000) / Math.Sqrt(part.PricePerUnit)),
                NormalStock = (int)((double)random.Next(3950, 4000) / Math.Sqrt(part.PricePerUnit)),
                RefillPeriodDays = 30
            });
        }
    }

    public void Seed (FwshDataContext context)
    {
        this.SeedWorkers(context, 14);
        this.SeedSuppliers(context, 6);
        this.SeedCustomers(context, 25);

        this.SeedColors(context);
        this.SeedFabricTypes(context);

        this.SeedParts(context);
        this.SeedMaterials(context);
        this.SeedFabrics(context);

        this.SeedStoredParts(context);
        this.SeedStoredMaterials(context);
        this.SeedStoredFabrics(context);
    }
}
