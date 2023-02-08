namespace Fwsh.MockData;

using System;

static class RandomExtensions
{
    public static T Choice<T>(this Random random, T[] data) 
    {
        return data[random.Next(0, data.Length)];
    }

    public static char Choice(this Random random, string data)
    {
        return data[random.Next(0, data.Length)];
    }
}
