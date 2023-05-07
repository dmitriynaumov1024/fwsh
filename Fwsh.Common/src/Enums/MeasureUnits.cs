namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class MeasureUnits
{
    public static readonly string 
        Unknown = null,
        Millimeters = "mm",
        Centimeters = "cm",
        Meters = "m",
        SquareMeters = "m2",
        CubicMeters = "m3",
        Liters = "L",
        Kilograms = "Kg",
        Grams = "g";

    public static readonly ICollection<string> KnownValues = new List<string> 
    {
        Unknown, Millimeters, Centimeters, Meters, 
        SquareMeters, CubicMeters, Liters, Kilograms, Grams
    };

    public static bool Contains(string value) => KnownValues.Contains(value);
}
