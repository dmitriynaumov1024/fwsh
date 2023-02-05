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
        var dimString = $"{length}x{width}x{height}";
        var dims = Dimensions.Parse(dimString);
        Console.Write("Source string: {0} \nParsed and back to string: {1}\n", 
                      dimString, dims.ToConventionalString());
        Assert.That(dims.Length == length && dims.Width == width && dims.Height == height);
        Assert.That(dims.ToConventionalString() == dimString);
    }
}
