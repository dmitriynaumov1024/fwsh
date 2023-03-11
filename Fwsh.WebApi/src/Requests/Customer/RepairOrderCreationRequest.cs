namespace Fwsh.WebApi.Requests.Customer;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class RepairOrderCreationRequest : Request
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
}
