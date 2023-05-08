namespace Fwsh.Tests;

using System;
using NUnit.Framework;
using Fwsh.Common;

[TestFixture]
public class PriceFormationTest
{
    [Test]
    public void TestPriceMargin()
    {
        int initialPrice = 1000;
        Console.Write("Price margin\n  {0} -> {1}\n", initialPrice, initialPrice.WithMargin());
        Assert.That(initialPrice.WithMargin(), Is.GreaterThan(initialPrice));
    }

    [Test]
    public void TestPriceDiscount()
    {
        int initialPrice = 1000;
        var customer = new Customer(){
            DiscountPercent = Random.Shared.Next(1, 20)
        };
        Console.Write("Price discount\n  discount: -{0}% \n  {1} -> {2} -> {3}\n", 
            customer.DiscountPercent, initialPrice, initialPrice.WithMargin(), 
            initialPrice.WithMargin().WithDiscountFor(customer));
    }

    [Test]
    public void TestDiscountLogicForIndividual()
    {
        int initialPrice = 1000;
        var customer = new Customer();
        for (int i=0; i<5; i++) {
            customer.ProdOrders.Add (new ProdOrder() { 
                Status = OrderStatus.Received
            });
        }
        customer.UpdateDiscountPercent();
        Console.Write("Discount for individual\n  with {4} orders\ndiscount: -{0}% \n  {1} -> {2} -> {3}\n", 
            customer.DiscountPercent, initialPrice, initialPrice.WithMargin(), 
            initialPrice.WithMargin().WithDiscountFor(customer), customer.ProdOrders.Count);
    }

    [Test]
    public void TestDiscountLogicForOrganization()
    {
        int initialPrice = 1000;
        var customer = new Customer() {
            IsOrganization = true
        };
        for (int i=0; i<5; i++) {
            customer.ProdOrders.Add (new ProdOrder() { 
                Status = OrderStatus.Received
            });
        }
        customer.UpdateDiscountPercent();
        Console.Write("Discount for organization\n  with {4} orders\ndiscount: -{0}% \n  {1} -> {2} -> {3}\n", 
            customer.DiscountPercent, initialPrice, initialPrice.WithMargin(), 
            initialPrice.WithMargin().WithDiscountFor(customer), customer.ProdOrders.Count);
    }
}
