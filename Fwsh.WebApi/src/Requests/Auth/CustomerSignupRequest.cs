namespace Fwsh.WebApi.Requests;

public class CustomerSignupRequest
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronym { get; set; }
    public string OrgName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
