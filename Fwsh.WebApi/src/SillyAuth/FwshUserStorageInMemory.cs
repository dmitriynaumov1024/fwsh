namespace Fwsh.WebApi.SillyAuth;

using System;
using System.Collections.Generic;
using System.Linq;

public class FwshUserStorageInMemory : FwshUserStorage
{
    private Dictionary<string, FwshUser> users = new();

    protected override FwshUser GetUserById (string id)
    {
        FwshUser result = null;
        users.TryGetValue(id, out result);
        return result;
    }

    protected override void PutUser (string id, FwshUser user) 
    {
        this.users[id] = user;
    }

    protected override void RemoveUser (string id)
    {
        this.users.Remove(id);
    }

    protected override bool ContainsUserId (string id)
    {
        return this.users.ContainsKey(id);
    }

    protected override int TryCleanUp ()
    {
        var now = DateTime.UtcNow;
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
