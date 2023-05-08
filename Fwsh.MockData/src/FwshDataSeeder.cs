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
    Factory<List<string>> Roles = new WorkerRoleFactory();
    Factory<string> OrgName = new OrgNameFactory();
    Factory<string> ExternalId = new ExternalIdFactory();
    Factory<Color> Color = new ColorFactory();
    Factory<FabricType> FabricType = new FabricTypeFactory();

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
            manager.Password = manager.Password.QuickHash();
            manager.Roles.Add(WorkerRoles.Management);
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
            worker.Password = worker.Password.QuickHash();
            worker.Roles = this.Roles.Next();
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
            customer.Password = customer.Password.QuickHash();
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

    Factory<StoredResource> Materials = new MaterialFactory();
    Factory<StoredResource> Parts = new PartFactory();

    void SeedStoredFabrics (FwshDataContext context)
    {
        var suppliers = context.Suppliers.Local.ToArray();

        var fabricTypesAndColors = context.Colors.Local
            .Join ( context.FabricTypes.Local, 
                color => !color.Name.Contains("wood"), fabric => true, 
                (color, fabric) => new Tuple<Color, FabricType>(color, fabric) 
            )
            .Where(_ => random.Probability(0.85))
            .ToList();

        foreach (var (color, fabricType) in fabricTypesAndColors) {
            context.StoredResources.Add ( new StoredResource {
                Type = ResourceTypes.Fabric,
                SlotName = SlotNames.Fabric,
                Color = color,
                FabricType = fabricType,
                Name = $"{fabricType.Name} {color.Name}",
                Description = fabricType.Description,
                PricePerUnit = random.Next(150, 290),
                MeasureUnit = MeasureUnits.SquareMeters,
                Precision = 1,
                PhotoUrl = "fabric-stub.jpg",
                Supplier = random.Choice(suppliers),
                ExternalId = this.ExternalId.Next(),
                InStock = random.Next(60, 98),
                NormalStock = 100,
                RefillPeriodDays = 30
            });
        }
    }

    void SeedStoredMaterials (FwshDataContext context)
    {
        var suppliers = context.Suppliers.Local.ToArray();

        var materials = this.Materials.All();

        foreach (var mat in materials) {
            context.StoredResources.Add ( new StoredResource {
                Type = ResourceTypes.Material,
                SlotName = mat.SlotName,
                Name = mat.Name,
                Description = mat.Description,
                Color = mat.Color,
                PhotoUrl = mat.PhotoUrl,
                PricePerUnit = mat.PricePerUnit,
                MeasureUnit = mat.MeasureUnit,
                Precision = mat.Precision,
                Supplier = random.Choice(suppliers),
                ExternalId = this.ExternalId.Next(),
                InStock = Math.Round((double)random.Next(4400, 5000) / Math.Sqrt(mat.PricePerUnit), mat.Precision),
                NormalStock = Math.Round((double)random.Next(4950, 6000) / Math.Sqrt(mat.PricePerUnit), mat.Precision),
                RefillPeriodDays = 14
            });
        }
    }

    void SeedStoredParts (FwshDataContext context)
    {
        var parts = this.Parts.All();
        var suppliers = context.Suppliers.Local.ToArray();

        foreach (var part in parts) {
            context.StoredResources.Add ( new StoredResource {
                Type = ResourceTypes.Part,
                Name = part.Name,
                Description = part.Description,
                PricePerUnit = part.PricePerUnit,
                MeasureUnit = null,
                Precision = 0,
                Supplier = random.Choice(suppliers),
                ExternalId = this.ExternalId.Next(),
                InStock = Math.Round((double)random.Next(1300, 3000) / Math.Sqrt(part.PricePerUnit)),
                NormalStock = Math.Round((double)random.Next(2900, 3000) / Math.Sqrt(part.PricePerUnit)),
                RefillPeriodDays = 30
            });
        }
    }

    void SeedDesigns (FwshDataContext context)
    {
        var designs = this.Designs.All();
        
        foreach (var design in designs) {
            design.PhotoUrls = Enumerable.Range(0, random.Next(2, 5))
                .Select(i => $"design-{design.Id}-{i}-{Guid.NewGuid()}.jpg").ToList();
        }

        context.Designs.AddRange(designs);
    }

    void SeedTaskPrototypes (FwshDataContext context)
    {
        var res = context.StoredResources.Local;

        var designs = context.Designs.Local.ToList();

        var screwPart = res.Where(p => p.Name.Contains("Screw 55mm")).FirstOrDefault();
        
        var wood = res.Where(m => m.Name == "Wood").FirstOrDefault();

        var woodenSlab = res.Where(m => m.Name.Contains("slab")).FirstOrDefault();

        var pvaGlue = res.Where(m => m.Name.Contains("PVA")).FirstOrDefault();

        var foam50 = res.Where(m => m.Name.Contains("Foam 50mm")).FirstOrDefault();

        var syntp = res.Where(m => m.Name == "Synthepone").FirstOrDefault();

        var interlin = res.Where(m => m.Name.Contains("Interlining")).FirstOrDefault();

        var foamGlue = res.Where(m => m.Name.Contains("Foam glue")).FirstOrDefault();

        var joints = res.Where(p => p.Name.Contains("mechanism")).ToArray();

        var leg = res.Where(p => p.Name.Contains("leg")).FirstOrDefault();

        foreach (var design in designs) 
        {
            var dimensions = design.DimExpanded ?? design.DimCompact 
                            ?? new Dimensions { Length = 100, Width = 100 };

            double approxArea = (double)dimensions.Length 
                              * (double)dimensions.Width / 10000;

            design.Tasks = new HashSet<TaskPrototype> {
                new TaskPrototype {
                    Precedence = 0,
                    Role = WorkerRoles.Carpentry,
                    Payment = 300,
                    Description = "Wooden frame manufacture",
                    Resources = new HashSet<ResourceQuantity> {
                        new ResourceQuantity {
                            Item = wood,
                            Quantity = approxArea * 0.01
                        },
                        new ResourceQuantity {
                            Item = pvaGlue,
                            Quantity = 0.075
                        },
                        new ResourceQuantity {
                            Item = screwPart,
                            Quantity = (int)approxArea * 10 + random.Next(0, 5)
                        }
                    }
                },
                new TaskPrototype {
                    Precedence = 1,
                    Role = WorkerRoles.Carpentry,
                    Payment = 300,
                    Description = "Compound frame manufacture",
                    Resources = new HashSet<ResourceQuantity> {
                        new ResourceQuantity {
                            Item = wood,
                            Quantity = approxArea * 0.005
                        },
                        new ResourceQuantity {
                            Item = woodenSlab,
                            Quantity = approxArea 
                        },
                        new ResourceQuantity {
                            Item = screwPart,
                            Quantity = (int)approxArea * 12 + random.Next(0, 5)
                        }
                    }
                },
                new TaskPrototype {
                    Precedence = 1,
                    Role = WorkerRoles.Sewing,
                    Payment = 300,
                    Description = "Cover manufacture",
                    Resources = new HashSet<ResourceQuantity> {
                        new ResourceQuantity {
                            SlotName = SlotNames.Fabric,
                            Quantity = approxArea * 2
                        }
                    } 
                },
                new TaskPrototype {
                    Precedence = 2,
                    Role = WorkerRoles.Upholstery,
                    Payment = 300,
                    Description = "Upholstery",
                    Resources = new HashSet<ResourceQuantity> {
                        new ResourceQuantity {
                            Item = foamGlue,
                            Quantity = 0.025 * approxArea
                        },
                        new ResourceQuantity {
                            Item = foam50,
                            Quantity = approxArea * 1.8
                        },
                        new ResourceQuantity {
                            Item = syntp,
                            Quantity = approxArea * 1.5
                        },
                        new ResourceQuantity {
                            Item = interlin,
                            Quantity = approxArea * 2
                        }
                    }
                },
                new TaskPrototype {
                    Precedence = 3,
                    Role = WorkerRoles.Assembly,
                    Payment = 250,
                    Description = "Final assembly",
                    Resources = new HashSet<ResourceQuantity> {
                        new ResourceQuantity {
                            Item = leg,
                            Quantity = random.Next(4, 6)
                        },
                        new ResourceQuantity {
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

        this.SeedStoredParts(context);
        this.SeedStoredMaterials(context);
        this.SeedStoredFabrics(context);

        this.SeedDesigns(context);
        this.SeedTaskPrototypes(context);
    }
}
