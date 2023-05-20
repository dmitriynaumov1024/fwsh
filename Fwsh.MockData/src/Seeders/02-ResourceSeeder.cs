namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.Utils;

public class ResourceSeeder : Seeder
{
    Random random = new Random();
    
    Factory<Color> Color = new ColorFactory();
    Factory<FabricType> FabricType = new FabricTypeFactory();
    Factory<Resource> Materials = new MaterialFactory();
    Factory<Resource> Parts = new PartFactory();
    Factory<string> ExternalId = new ExternalIdFactory();

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
        var suppliers = context.Suppliers.Local.ToArray();

        var fabricTypesAndColors = context.Colors.Local
            .Join ( context.FabricTypes.Local, 
                color => !color.Name.Contains("wood"), fabric => true, 
                (color, fabric) => new Tuple<Color, FabricType>(color, fabric) 
            )
            .Where(_ => random.Probability(0.85))
            .ToList();

        foreach (var (color, fabricType) in fabricTypesAndColors) {
            context.Resources.Add ( new Resource {
                Type = ResourceTypes.Fabric,
                SlotName = SlotNames.Fabric,
                Color = color,
                FabricType = fabricType,
                Name = $"{fabricType.Name} {color.Name}",
                Description = fabricType.Description,
                PhotoUrl = "fabric-stub.jpg",
                PricePerUnit = random.Next(150, 290),
                MeasureUnit = MeasureUnits.SquareMeters,
                Precision = 1,
                Stored = new StoredResource { 
                    Supplier = random.Choice(suppliers),
                    ExternalId = this.ExternalId.Next(),
                    InStock = random.Next(60, 98),
                    NormalStock = 100,
                    RefillPeriodDays = 30
                }
            });
        }
    }

    void SeedMaterials (FwshDataContext context)
    {
        var suppliers = context.Suppliers.Local.ToArray();

        var materials = this.Materials.All();

        foreach (var mat in materials) {
            context.Resources.Add ( new Resource {
                Type = ResourceTypes.Material,
                SlotName = mat.SlotName,
                Name = mat.Name,
                Description = mat.Description,
                Color = mat.Color,
                PhotoUrl = mat.PhotoUrl,
                PricePerUnit = mat.PricePerUnit,
                MeasureUnit = mat.MeasureUnit,
                Precision = mat.Precision,
                Stored = new StoredResource {
                    Supplier = random.Choice(suppliers),
                    ExternalId = this.ExternalId.Next(),
                    InStock = Math.Round((double)random.Next(400, 5000) / Math.Sqrt(mat.PricePerUnit), mat.Precision),
                    NormalStock = Math.Round((double)random.Next(4950, 6000) / Math.Sqrt(mat.PricePerUnit), mat.Precision),
                    RefillPeriodDays = 14
                }
            });
        }
    }

    void SeedParts (FwshDataContext context)
    {
        var parts = this.Parts.All();
        var suppliers = context.Suppliers.Local.ToArray();

        foreach (var part in parts) {
            context.Resources.Add ( new Resource {
                Type = ResourceTypes.Part,
                Name = part.Name,
                Description = part.Description,
                PricePerUnit = part.PricePerUnit,
                MeasureUnit = null,
                Precision = 0,
                Stored = new StoredResource {
                    Supplier = random.Choice(suppliers),
                    ExternalId = this.ExternalId.Next(),
                    InStock = Math.Round((double)random.Next(130, 3000) / Math.Sqrt(part.PricePerUnit)),
                    NormalStock = Math.Round((double)random.Next(2900, 3000) / Math.Sqrt(part.PricePerUnit)),
                    RefillPeriodDays = 30
                }
            });
        }
    }

    void SeedSupplyOrders (FwshDataContext context)
    {
        var resources = context.Resources.Local
            .Where(res => res.Stored.NeedsRefill)
            .Where(_ => random.Probability(0.6))
            .ToList();

        foreach (var res in resources) {
            context.SupplyOrders.Add ( new SupplyOrder {
                Item = res,
                Supplier = res.Stored.Supplier,
                ExpectQuantity = res.Stored.NormalStock,
                ExpectPricePerUnit = res.PricePerUnit,
                Status = OrderStatus.Submitted,
                IsActive = true
            });
            res.Stored.SupplyOrderCount += 1;
        }
    }

    public override void Seed (FwshDataContext context)
    {
        this.SeedColors(context);
        this.SeedFabricTypes(context);

        this.SeedFabrics(context);
        this.SeedMaterials(context);
        this.SeedParts(context);

        this.SeedSupplyOrders(context);
    }

}
