namespace Fwsh.WebApi.Requests.Worker;

using Fwsh.Common;
using Fwsh.Utils;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class WorkerUpdateRequest : Request, UpdateRequest<Worker>
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("newPassword", this.NewPassword)
                .NotNull().LengthInRange(8, 64);
    }

    public void ApplyTo (Worker worker)
    {
        worker.Password = this.NewPassword.QuickHash();
    }
}
