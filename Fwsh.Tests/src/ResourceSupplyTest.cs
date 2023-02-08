namespace Fwsh.Tests;

using System;
using System.Text.Json;
using NUnit.Framework;
using Fwsh.Common;

[TestFixture]
public class ResourceSupplyTest
{
    public static JsonSerializerOptions jsonOptions = new JsonSerializerOptions {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
        WriteIndented = true
    };

    [Test]
    public void TestResourceSupplyOrder()
    {
        var supplier = new Supplier {
            Id = 1,
            Surname = "Bandera",
            Name = "Stepan",
            Patronym = "Andriyovych",
            Phone = "1923840148",
            Email = "bandera128@mail.com",
            OrgName = "УПА"
        };

        var part = new Part {
            Id = 1,
            Name = "Bolt.6.40",
            Description = "Bolt",
            PricePerUnit = 1.15
        };

        var storedPart = new StoredPart {
            Id = 1,
            Item = part,
            SupplierId = 1,
            Supplier = supplier,
            ExternalId = "021.0400(6x40)",
            Quantity = 32,
            NormalStock = 200,
            RefillPeriodDays = 30
        };

        var supplyOrder = new PartSupplyOrder(storedPart);

        Console.WriteLine("Test supply order");
        Console.WriteLine(JsonSerializer.Serialize(supplier, jsonOptions));
        Console.WriteLine(JsonSerializer.Serialize(part, jsonOptions));
        Console.WriteLine(JsonSerializer.Serialize(storedPart, jsonOptions));
        Console.WriteLine(JsonSerializer.Serialize(supplyOrder, jsonOptions));

        Assert.AreEqual(supplyOrder.ItemId, part.Id);
    }
}
