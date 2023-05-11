namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Validation;

public class MaterialRequest : ResourceRequest
{
    public int? ColorId { get; set; }

    public override string Type => ResourceTypes.Material;

    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);

        if (this.SlotName == SlotNames.Decor) {
            validator.Property("colorId", this.ColorId).NotNull();
        }
        else if (this.SlotName != null) {
            validator.Property("slotName", this.SlotName).Condition(false);
        }
    }
    
    public override void ApplyTo (Resource mat)
    {
        base.ApplyTo(mat);
        mat.ColorId = this.ColorId;
    }
}
