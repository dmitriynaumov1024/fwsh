namespace Fwsh.WebApi.Requests.Manager;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public class RepairTaskRequest : Request, 
    CreationRequest<RepairTask>, UpdateRequest<RepairTask>
{
    public int OrderId { get; set; }
    public string RoleName { get; set; }
    public int Payment { get; set; }
    public string Status { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("roleName", this.RoleName)
            .Condition(Roles.KnownWorkerRoles.Contains(this.RoleName));

        validator.Property("payment", this.Payment)
            .ValueInRange(1, 999);

        validator.Property("status", this.Status)
            .Condition(this.Status == null || TaskStatus.Contains(this.Status));
    }

    public RepairTask Create()
    {
        var result = new RepairTask();
        this.ApplyTo(result);
        return result;
    }

    public void ApplyTo (RepairTask task)
    {
        task.OrderId = this.OrderId;
        task.RoleName = this.RoleName;
        task.Payment = this.Payment;
        task.Status = this.Status ?? TaskStatus.Unknown;
    }
}
