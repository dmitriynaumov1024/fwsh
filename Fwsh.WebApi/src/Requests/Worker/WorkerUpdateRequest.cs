namespace Fwsh.WebApi.Requests.Worker;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;

public class WorkerUpdateRequest : Request
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    public override Result Validate()
    {
        return (Result) true;
    }
}
