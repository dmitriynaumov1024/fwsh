namespace Fwsh.WebApi.Requests.Customer;

public class CustomerUpdateRequest 
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
