namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.Utils;

public class ProductionSeeder : Seeder
{
    Random random = new Random();

    void SeedProdOrders (FwshDataContext context)
    {
        var fabrics = context.Resources.Local
            .Where(r => r.SlotName == SlotNames.Fabric)
            .ToArray();

        var decors = context.Resources.Local
            .Where(r => r.SlotName == SlotNames.Decor)
            .ToArray();

        var designs = context.Designs.Local.ToArray();

        var customers = context.Customers.Local.ToList();

        foreach (var customer in customers) {
            customer.ProdOrders.AddRange ( 
                Enumerable.Range(0, random.Next(2, 4)).Select(_ => {
                    Design design = random.Choice(designs);
                    int quantity = random.Next(1, 5);
                    bool orderIsActive = random.Probability(0.40);
                    Resource fabric = random.Choice(fabrics);
                    Resource decor = random.Choice(decors);
                    int fixedPrice = (int)(design.Price 
                        + fabric.PricePerUnit * design.FabricUsage 
                        + decor.PricePerUnit * design.DecorUsage); 
                    return new ProdOrder() {
                        Customer = customer,
                        Design = design,
                        Decor = (design.DecorUsage > 0)? random.Choice(decors) : null,
                        Fabric = random.Choice(fabrics),
                        Quantity = quantity,
                        PricePerOne = fixedPrice,
                        Price = fixedPrice * quantity,
                        IsActive = orderIsActive,
                        Status = orderIsActive? OrderStatus.Submitted : OrderStatus.Unknown
                    };
                })
            );
        }
    }

    void SeedFurniture (FwshDataContext context)
    {
        var orders = context.ProdOrders.Local
            .Where(o => o.Status == OrderStatus.Submitted)
            .Where(_ => random.Probability(0.78))
            .ToList();

        foreach (var order in orders) {
            order.Status = OrderStatus.Delayed;
            order.Furnitures.AddRange (
                Enumerable.Range(0, order.Quantity).Select(_ => {
                    return order.Design.CreateFurniture(order);
                })
            );
        }
    }

    void SeedProdNotifications (FwshDataContext context)
    {
        var orders = context.ProdOrders.Local.ToList();

        foreach (var order in orders) {
            order.Notifications.AddRange (
                Enumerable.Range(0, random.Next(0, 5))
                .Select(_ => new ProdNotification() {
                    Text = $"Example notification text #{random.Next(10000, 999999)}",
                    IsRead = false
                })
            );
        }
    }

    public override void Seed (FwshDataContext context)
    {
        this.SeedProdOrders(context);
        this.SeedFurniture(context);
        this.SeedProdNotifications(context);
    }
}
