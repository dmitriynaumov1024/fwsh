namespace Fwsh.WebApi.SillyAuth;

using System;

public class FwshUser
{
    public int ConfirmedId { get; set; }
    public UserRole ConfirmedRole { get; set; }
    public string Token { get; set; }
    public string OldToken { get; set; }
    public int TTL { get; set; }
    public DateTime LastTokenUpdate { get; set; }
}
