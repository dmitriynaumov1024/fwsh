namespace Fwsh.Database;

using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Fwsh.Common;

public class DimensionsConverter : ValueConverter<Dimensions, string>
{
    public DimensionsConverter() : base (
        dim => Dimensions.StringifyWLH(dim),
        str => Dimensions.ParseWLH(str)
    ) { }
}
