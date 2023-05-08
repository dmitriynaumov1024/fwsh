namespace Fwsh.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class env 
{
    static IDictionary<string, string> readDotenvVariables ()
    {
        try {
            return File.ReadLines(".env")
                .Select(line => line.Split('#')[0].Split('='))
                .Where(words => words.Length == 2)
                .ToDictionary(kvpair => kvpair[0].Trim(), kvpair => kvpair[1].Trim());
        }
        catch (Exception) {
            return new Dictionary<string, string>();
        }
    }

    static readonly IDictionary<string, string> dotenvVariables = readDotenvVariables();
    
    public static string get (string key) 
    {
        string result;
        dotenvVariables.TryGetValue(key, out result); 
        return result ?? Environment.GetEnvironmentVariable(key);
    }

    public static bool isTrue (string key)
    {
        string result = get(key)?.ToLower();
        return result == "true" || result == "1";
    }

    public static bool isDevelopment = get("ENVIRONMENT_TYPE")?.ToLower() == "development";
}
