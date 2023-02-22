namespace Fwsh.WebApi.Requests;

public class CustomerUpdateRequest 
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
