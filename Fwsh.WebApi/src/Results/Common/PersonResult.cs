namespace Fwsh.WebApi.Results.Common;

using System;
using Fwsh.Common;

public class PersonResult
{
    public int Id { get; set; }

    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    
    public string Phone { get; set; }
    public string Email { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public PersonResult (Person person)
    {
        this.Id = person.Id;
        this.Surname = person.Surname;
        this.Name = person.Name;
        this.Patronym = person.Patronym;
        this.Phone = person.Phone;
        this.Email = person.Email;
        this.CreatedAt = person.CreatedAt;
    }
}
