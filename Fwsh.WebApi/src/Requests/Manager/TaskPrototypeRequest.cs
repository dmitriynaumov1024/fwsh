namespace Fwsh.WebApi.Requests.Manager;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public class TaskPrototypeRequest : Request, 
    CreationRequest<TaskPrototype>, UpdateRequest<TaskPrototype>
{
    public int Precedence { get; set; }
    public string Role { get; set; }
    public int Payment { get; set; }
    public string Description { get; set; }

    public List<ResourceQuantity> Resources { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("precedence", this.Precedence)
            .ValueInRange(0, 20);

        validator.Property("role", this.Role)
            .Condition(WorkerRoles.KnownWorkerRoles.Contains(this.Role));

        validator.Property("payment", this.Payment)
            .ValueInRange(1, 9999);

        validator.Property("description", this.Description)
            .NotNull().LengthInRange(9, 2000);
    }

    public TaskPrototype Create()
    {
        var result = new TaskPrototype();
        this.ApplyTo(result);
        return result;
    }

    public void ApplyTo (TaskPrototype task)
    {
        task.Role = this.Role;
        task.Precedence = this.Precedence;
        task.Payment = this.Payment;
        task.Description = this.Description;

        foreach (var res in this.Resources) {
            if (task.Resources.FirstOrDefault(r => r.ItemId == res.ItemId || 
                (r.SlotName != null && r.SlotName == res.SlotName)) is ResourceQuantity resource) {
                if (res.ExpectQuantity <= 0) {
                    task.Resources.Remove(resource);
                }
                else {
                    resource.ExpectQuantity = res.ExpectQuantity;
                }
            }
            else if (res.ExpectQuantity > 0) {
                task.Resources.Add(new ResourceQuantity() {
                    ItemId = res.ItemId,
                    SlotName = res.SlotName,
                    ExpectQuantity = res.ExpectQuantity,
                    ActualQuantity = 0
                });
            }
        }
    }
}
