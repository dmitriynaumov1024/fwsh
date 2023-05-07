namespace Fwsh.Database;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Fwsh.Common;

public class StringListConverter : ValueConverter<List<string>, string>
{
    public StringListConverter() : base (
        strings => string.Join(";", strings),
        str => new List<string>(str.Split(";", StringSplitOptions.None))
    ) { }
}
