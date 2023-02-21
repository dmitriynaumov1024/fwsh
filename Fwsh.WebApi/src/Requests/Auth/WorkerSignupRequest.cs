namespace Fwsh.WebApi.Requests;

using System;
using System.Collections.Generic;

public class WorkerSignupRequest
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; }
}
