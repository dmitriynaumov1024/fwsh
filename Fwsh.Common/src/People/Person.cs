namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class Person
{
    public int Id { get; set; }

    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }

    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
