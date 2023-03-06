namespace Fwsh.WebApi.Requests.Customer;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;

public class CustomerUpdateRequest : Request
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    public override Result Validate()
    {
        return (Result) true;
    } 
}
