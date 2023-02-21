namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.Utils;

public class FwshDataSeeder
{
    public Logger CredentialsLogger { get; set; }

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
    Factory<Design> Designs = new DesignFactory();

    void SeedManagers (FwshDataContext context, int count)
    {
        for (int i=0; i<count; i++) {
            var (surname, name, patronym) = this.FullName.Next();
            var manager = new Worker {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next(),
                Password = this.Password.Next()
            };
            this.CredentialsLogger?.Log("manager, {0}, {1}", manager.Phone, manager.Password);
            manager.Password = manager.Password.SHA512Hash();
            manager.Roles.Add(new WorkerRole { RoleName = Roles.Management });
            context.Workers.Add(manager);
        }
    }

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
            this.CredentialsLogger?.Log("worker, {0}, {1}", worker.Phone, worker.Password);
            worker.Password = worker.Password.SHA512Hash();
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
            var customer = new Customer {
                Surname = surname,
                Name = name,
                Patronym = patronym,
                Phone = this.PhoneNumber.Next(),
                Email = this.Email.Next(),
                Password = this.Password.Next()
            };
            this.CredentialsLogger?.Log("customer, {0}, {1}", customer.Phone, customer.Password);
            customer.Password = customer.Password.SHA512Hash();
            context.Customers.Add(customer);
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

    void SeedDesigns (FwshDataContext context)
    {
        var designs = this.Designs.All();
        
        foreach (var design in designs) {
            var photos = Enumerable.Range(0, random.Next(2, 5))
                .Select(i => new DesignPhoto {
                    Position = i,
                    Url = $"/{design.NameKey}{i}.jpg"
                });
            design.Photos = new HashSet<DesignPhoto>(photos);
        }

        context.Designs.AddRange(designs);
    }

    void SeedTaskPrototypes (FwshDataContext context)
    {
        var designs = context.Designs.Local.ToList();
        
        // context.Fabrics.Add ( new Fabric {
        //     Id = -1,
        //     Name = "default",
        //     ColorId = -1,
        //     FabricTypeId = -1
        // });

        var screwPart = context.Parts.Local
            .Where(p => p.Name.Contains("Screw 55mm")).FirstOrDefault();
        
        var wood = context.Materials.Local
            .Where(m => m.Name == "Wood").FirstOrDefault();

        var woodenSlab = context.Materials.Local
            .Where(m => m.Name.Contains("slab")).FirstOrDefault();

        var pvaGlue = context.Materials.Local
            .Where(m => m.Name.Contains("PVA")).FirstOrDefault();

        var foam50 = context.Materials.Local
            .Where(m => m.Name.Contains("Foam 50mm")).FirstOrDefault();

        var syntp = context.Materials.Local
            .Where(m => m.Name == "Synthepone").FirstOrDefault();

        var interlin = context.Materials.Local
            .Where(m => m.Name.Contains("Interlining")).FirstOrDefault();

        var foamGlue = context.Materials.Local
            .Where(m => m.Name.Contains("Foam glue")).FirstOrDefault();

        var joints = context.Parts.Local
            .Where(p => p.Name.Contains("mechanism")).ToArray();

        var leg = context.Parts.Local
            .Where(p => p.Name.Contains("leg")).FirstOrDefault();

        foreach (var design in designs) 
        {
            var dimensions = design.DimExpanded ?? design.DimCompact 
                            ?? new Dimensions { Length = 100, Width = 100 };

            double approxArea = (double)dimensions.Length 
                              * (double)dimensions.Width / 10000;

            design.TaskPrototypes = new HashSet<TaskPrototype> {
                new TaskPrototype {
                    Precedence = 0,
                    RoleName = Roles.Carpentry,
                    Payment = 300,
                    Description = "Wooden frame manufacture",
                    InstructionUrl = $"/{design.NameKey}-task-0.pdf",
                    Materials = new HashSet<TaskMaterial> {
                        new TaskMaterial {
                            Item = wood,
                            Quantity = approxArea * 0.01
                        },
                        new TaskMaterial {
                            Item = pvaGlue,
                            Quantity = 0.075
                        }
                    },
                    Parts = new HashSet<TaskPart> {
                        new TaskPart {
                            Item = screwPart,
                            Quantity = (int)(approxArea * 10) + random.Next(0, 5)
                        }
                    }
                },
                new TaskPrototype {
                    Precedence = 1,
                    RoleName = Roles.Carpentry,
                    Payment = 300,
                    Description = "Compound frame manufacture",
                    InstructionUrl = $"/{design.NameKey}-task-1.pdf",
                    Materials = new HashSet<TaskMaterial> {
                        new TaskMaterial {
                            Item = wood,
                            Quantity = approxArea * 0.005
                        },
                        new TaskMaterial {
                            Item = woodenSlab,
                            Quantity = approxArea 
                        }
                    },
                    Parts = new HashSet<TaskPart> {
                        new TaskPart {
                            Item = screwPart,
                            Quantity = (int)(approxArea * 12) + random.Next(0, 5)
                        }
                    }
                },
                new TaskPrototype {
                    Precedence = 1,
                    RoleName = Roles.Sewing,
                    Payment = 300,
                    Description = "Cover manufacture",
                    InstructionUrl = $"/{design.NameKey}-task-2.pdf",
                    Fabrics = new HashSet<TaskFabric> {
                        new TaskFabric {
                            // FabricId = -1,
                            Quantity = approxArea * 2
                        }
                    } 
                },
                new TaskPrototype {
                    Precedence = 2,
                    RoleName = Roles.Upholstery,
                    Payment = 300,
                    Description = "Upholstery",
                    InstructionUrl = $"/{design.NameKey}-task-3.pdf",
                    Materials = new HashSet<TaskMaterial> {
                        new TaskMaterial {
                            Item = foamGlue,
                            Quantity = 0.025 * approxArea
                        },
                        new TaskMaterial {
                            Item = foam50,
                            Quantity = approxArea * 1.8
                        },
                        new TaskMaterial {
                            Item = syntp,
                            Quantity = approxArea * 1.5
                        },
                        new TaskMaterial {
                            Item = interlin,
                            Quantity = approxArea * 2
                        }
                    }
                },
                new TaskPrototype {
                    Precedence = 3,
                    RoleName = Roles.Assembly,
                    Payment = 250,
                    Description = "Final assembly",
                    InstructionUrl = $"/{design.NameKey}-task-4.pdf",
                    Parts = new HashSet<TaskPart> {
                        new TaskPart {
                            Item = leg,
                            Quantity = random.Next(4, 6)
                        },
                        new TaskPart {
                            Item = random.Choice(joints),
                            Quantity = 2
                        }
                    }
                }
            };
        }
    }

    public void Seed (FwshDataContext context)
    {
        this.SeedManagers(context, 2);
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

        this.SeedDesigns(context);
        this.SeedTaskPrototypes(context);
    }
}
