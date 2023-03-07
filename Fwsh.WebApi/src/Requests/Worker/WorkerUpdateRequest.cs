namespace Fwsh.WebApi.Requests.Worker;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class WorkerUpdateRequest : Request
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.DoNothing();
    }
}
