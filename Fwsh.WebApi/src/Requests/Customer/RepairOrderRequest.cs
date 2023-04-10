namespace Fwsh.WebApi.Requests.Customer;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class RepairOrderRequest : Request, UpdateRequest<RepairOrder>
{
    public string Description { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("description", this.Description) 
                .NotNull().LengthInRange(100, 10000);
    }

    public void ApplyTo (RepairOrder order)
    {
        order.Description = this.Description;
    } 
}
