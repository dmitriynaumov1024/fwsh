namespace Fwsh.WebApi.SillyAuth;

using System;
using System.Collections.Generic;
using System.Linq;

public abstract class FwshUserStorage
{
    protected abstract FwshUser GetUserById (string id);
    protected abstract void PutUser (string id, FwshUser user);
    protected abstract void RemoveUser (string id);
    protected abstract bool ContainsUserId (string id);
    protected abstract int TryCleanUp ();

    protected readonly TimeSpan CleanUpInterval = new TimeSpan(0, 10, 0); // 10 minutes
    protected readonly TimeSpan SessionTimeout = new TimeSpan(4, 0, 0); // 4 hours
    protected readonly TimeSpan TokenTimeTTL = new TimeSpan(0, 10, 0); // 10 minutes
    protected readonly int TokenRequestTTL = 16; // 16 requests and then token should be updated

    private DateTime LastCleanUpTime = DateTime.UtcNow;
    
    public FwshUser GetUserByIdToken (string id, string token)
    {
        // 1. Null guard
        if (id == null || token == null) {
            return null;
        } 

        // 2. Try get value
        FwshUser result = this.GetUserById(id);
        
        if (result == null) {
            return null;
        }

        // 3. If user session is expired, terminate anyway
        var now = DateTime.UtcNow;
        if (now - result.LastTokenUpdate >= SessionTimeout) {
            this.OnExpiredUser(result);
            return null;
        }

        // 4. Check token
        if (!(result.Token == token || result.OldToken == token)) {
            return null;
        }

        // 5. If request TTL or time TTL reaches 0, update token
        result.TTL--;
        if (result.TTL <= 0 || now - result.LastTokenUpdate >= TokenTimeTTL) {
            result.TTL = TokenRequestTTL;
            result.LastTokenUpdate = now;
            result.OldToken = result.Token;
            result.Token = Guid.NewGuid().ToString();
            this.PutUser(id, result);
        }

        // 6. return result
        return result;
    }

    public KeyValuePair<string, FwshUser> NewUser ()
    {
        FwshUser user = new() {
            Token = Guid.NewGuid().ToString(),
            LastTokenUpdate = DateTime.UtcNow,
            TTL = TokenRequestTTL
        };
        string id = null;
        do { 
            id = Guid.NewGuid().ToString();
        } while (this.ContainsUserId(id));
        this.PutUser(id, user);
        return new (id, user);
    }

    public void UpdateUser (string id, FwshUser user)
    {
        this.PutUser(id, user);
    }

    protected void OnExpiredUser (FwshUser user)
    {
        if (DateTime.UtcNow - this.LastCleanUpTime <= CleanUpInterval) return;
        int expiredCleanedUp = this.TryCleanUp();
        this.LastCleanUpTime = DateTime.UtcNow;
        Console.WriteLine("  Cleaned up {0} expired users", expiredCleanedUp);
    }
}
