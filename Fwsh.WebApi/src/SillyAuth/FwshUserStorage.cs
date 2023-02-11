namespace Fwsh.WebApi.SillyAuth;

using System;
using System.Collections.Generic;
using System.Linq;

public class FwshUserStorage
{
    private static TimeSpan SessionTimeout = new TimeSpan(4, 0, 0); // 4 hours
    private static TimeSpan TokenTimeTTL = new TimeSpan(0, 10, 0); // 10 minutes
    private static int TokenRequestTTL = 16; // 16 requests and then token should be updated

    private static Dictionary<string, FwshUser> users = new();

    public static FwshUser GetUserById (string id)
    {
        FwshUser result = null;
        users.TryGetValue(id, out result);
        return result;
    }

    public static FwshUser GetUserByIdToken (string id, string token)
    {
        var now = DateTime.UtcNow;

        FwshUser result = null;
        users.TryGetValue(id, out result);
        
        if (result == null) {
            return null;
        }

        if (now - result.LastTokenUpdate >= SessionTimeout) {
            int expiredCleanedUp = TryCleanUp();
            Console.WriteLine("  Cleaned up {0} expired users", expiredCleanedUp);
            return null;
        }

        if (result.Token != token) {
            return null;
        }

        result.TTL--;
        if (result.TTL <= 0 || now - result.LastTokenUpdate >= TokenTimeTTL) {
            result.TTL = TokenRequestTTL;
            result.LastTokenUpdate = DateTime.UtcNow;
            result.Token = Guid.NewGuid().ToString();
        }

        return result;
    }

    public static KeyValuePair<string, FwshUser> NewUser ()
    {
        FwshUser user = new() {
            Token = Guid.NewGuid().ToString(),
            LastTokenUpdate = DateTime.UtcNow,
            TTL = TokenRequestTTL
        };
        string key = null;
        do { 
            key = Guid.NewGuid().ToString();
        } while (users.ContainsKey(key));
        users[key] = user;
        return new (key, user);
    }

    private static DateTime LastCleanUpTime = DateTime.UtcNow;
    private static TimeSpan CleanUpInterval = new TimeSpan(0, 10, 0);
    
    public static int TryCleanUp ()
    {
        if (DateTime.UtcNow - LastCleanUpTime <= CleanUpInterval) return 0;

        var now = DateTime.UtcNow;
        LastCleanUpTime = now;
        var expired = users
            .Where(kv => now - kv.Value.LastTokenUpdate >= SessionTimeout)
            .Select(kv => kv.Key)
            .ToList();
        foreach (var key in expired) {
            users.Remove(key);
        }
        return expired.Count;
    }
}
