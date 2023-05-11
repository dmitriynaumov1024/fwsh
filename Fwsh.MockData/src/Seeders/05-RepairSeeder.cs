namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.Utils;

public class RepairSeeder : Seeder
{
    Random random = new Random();

    void SeedRepairOrders (FwshDataContext context)
    {
        var customers = context.Customers.Local.ToList();

        foreach (var customer in customers)
        {
            customer.RepairOrders.AddRange (
                Enumerable.Range(0, random.Next(1, 3)).Select (_ => new RepairOrder {
                    Customer = customer,
                    Description = $"This is an example of repair order #{random.Next(10000, 99999)}",
                    PhotoUrls = new[] { "repairorder-stub.jpg" }
                })
            );
        }
    }


    void SeedRepairTasks (FwshDataContext context)
    {
        var orders = context.RepairOrders.Local.ToList();

        foreach (var order in orders) {
            order.Tasks.Add ( new RepairTask {
                Description = "Disassemble and assemble again, idk",
                Payment = random.Next(15, 31) * 10,
                Role = WorkerRoles.Assembly,
                Status = TaskStatus.Unknown
            });
        }
    }

    void SeedRepairNotifications (FwshDataContext context)
    {
        var orders = context.RepairOrders.Local.ToList();

        foreach (var order in orders) {
            order.Notifications.AddRange (
                Enumerable.Range(0, random.Next(0, 5))
                .Select(_ => new RepairNotification() {
                    Text = $"Example notification text #{random.Next(10000, 999999)}",
                    IsRead = false
                })
            );
        }
    }

    public override void Seed (FwshDataContext context)
    {
        this.SeedRepairOrders(context);
        this.SeedRepairTasks(context);
        this.SeedRepairNotifications(context);
    }
}
