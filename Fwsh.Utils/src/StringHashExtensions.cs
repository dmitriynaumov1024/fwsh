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
}
