namespace Fwsh.WebApi.Requests.Customer;

using System;
using Fwsh.Common;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class RepairOrderRequest : Request, UpdateRequest<RepairOrder>
{
    public string Description { get; set; }
    public int PhotoCount { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("description", this.Description) 
                .NotNull().LengthInRange(100, 10000);

        validator.Property("photoCount", this.PhotoCount)
                .ValueInRange(2, 8);
    }

    public void ApplyTo (RepairOrder order)
    {
        order.Description = this.Description;
        
        for (int i = order.Photos.Count; i<this.PhotoCount; i++) {
            order.Photos.Add(new RepairOrderPhoto {
                Position = i,
                Url = ""
            });
        }
    } 
}
