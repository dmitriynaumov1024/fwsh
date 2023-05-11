namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.Utils;

public class BlueprintSeeder : Seeder
{
    Random random = new Random();
    
    Factory<Design> Designs = new DesignFactory();

    void SeedDesigns (FwshDataContext context)
    {
        var designs = this.Designs.All();
        
        int count = 0;
        
        foreach (var design in designs) {
            count++;
            design.PhotoUrls = Enumerable.Range(0, random.Next(2, 5))
                .Select(i => $"design-{count}-{i}-{Guid.NewGuid()}.jpg").ToList();
        }

        context.Designs.AddRange(designs);
    }

    void SeedTaskPrototypes (FwshDataContext context)
    {
        var res = context.Resources.Local;

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

            if (design.Type == FurnitureTypes.Ottoman) {
                design.Tasks.Add ( new TaskPrototype {
                    Precedence = 2,
                    Role = WorkerRoles.Carpentry,
                    Payment = 100,
                    Description = "Decorative panels manufacture",
                    Resources = new HashSet<ResourceQuantity> {
                        new ResourceQuantity {
                            SlotName = SlotNames.Decor,
                            Quantity = approxArea * 0.1
                        }
                    }
                });
            }
        }
    }

    public override void Seed (FwshDataContext context)
    {
        this.SeedDesigns(context);
        this.SeedTaskPrototypes(context);
    }
}
