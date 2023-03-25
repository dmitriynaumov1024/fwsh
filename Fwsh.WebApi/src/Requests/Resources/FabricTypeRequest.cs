namespace Fwsh.WebApi.Requests.Resources;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Validation;

public class FabricTypeRequest : Request, CreationRequest<FabricType>, UpdateRequest<FabricType>
{
    public string Name { get; set; }
    public string Description { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("name", this.Name)
                .NotNull().LengthInRange(3, 40);

        validator.Property("description", this.Description)
                .NotNull().LengthInRange(3, 200);
    }

    public FabricType Create()
    {
        return new FabricType() {
            Name = this.Name,
            Description = this.Description
        };
    }

    public void ApplyTo (FabricType ftype)
    {
        ftype.Name = this.Name;
        ftype.Description = this.Description;
    }

}
