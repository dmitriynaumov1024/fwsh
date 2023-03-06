namespace Fwsh.WebApi.Requests.Auth;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.Utils;

public class WorkerSignupRequest : Request
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; } = new List<string>();

    public override Result Validate()
    {
        return Validator()
            .Assert("surname", this.Surname != null && this.Surname.Length.InRange(2, 30))
            .Assert("name", this.Name != null && this.Name.Length.InRange(2, 24))
            .Assert("patronym", this.Patronym != null && this.Patronym.Length.InRange(5, 28))
            .Assert("phone", this.Phone != null && PhoneRegex.IsMatch(this.Phone))
            .Assert("email", this.Email != null && EmailRegex.IsMatch(this.Email))
            .Assert("password", this.Password != null && this.Password.Length.InRange(8, 64))
            .Assert("roles", this.Roles != null && this.Roles.Count.InRange(0, 4))
            .GetVerdict();
    }

    static Regex PhoneRegex = new Regex(@"^\+?[0-9]{10,14}$");
    static Regex EmailRegex = new Regex(@"^[a-z0-9-]{2,64}@([a-z0-9-]{1,60}.){1,5}[a-z0-9]{2,10}$");
}
