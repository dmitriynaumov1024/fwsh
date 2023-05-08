namespace Fwsh.Tests;

using System;
using NUnit.Framework;
using Fwsh.Common;

[TestFixture]
public class DimensionsTest
{
    [Test]
    public void TestParseDimensions() 
    {
        int length = Random.Shared.Next(1, 9999), 
            width = Random.Shared.Next(1, 9999), 
            height = Random.Shared.Next(1, 1000);
        var dimString = $"{width};{length};{height};cm";
        var dims = Dimensions.ParseWLH(dimString);
        Console.Write("Dimensions \n  Source string: {0} \n  Parsed and back to string: {1}\n", 
                      dimString, Dimensions.StringifyWLH(dims));
        Assert.That(dims.Length == length && dims.Width == width && dims.Height == height && dims.MeasureUnit == "cm");
        Assert.That(Dimensions.StringifyWLH(dims) == dimString);
    }
}
