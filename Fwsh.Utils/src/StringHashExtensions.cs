namespace Fwsh.Utils;

using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

public static class StringHashExtensions
{
    public static string SHA512Hash (this string source)
    {
        if (source == null) return null;
        
        if (source == String.Empty) return String.Empty;

        using (var sha512 = SHA512.Create()) {
            return Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(source)));
        }
    }

    public static string QuickHash (this string source)
    {
        if (source == null) return null;
        
        if (source == String.Empty) return String.Empty;

        int length = source.Length;

        Span<byte> bytes = stackalloc byte[length * 2];

        for (int i=0; i<length; i++) {
            bytes[i*2] = (byte)source[i];
            bytes[i*2+1] = (byte)(source[i]>>8);
        }

        Span<byte> result = stackalloc byte[48];

        SHA384.HashData(bytes, result);

        return Convert.ToBase64String(result);
    }
}
