namespace Fwsh.WebApi.Requests.Resources;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Validation;

public class StoredFabricRequest : ResourceRequest
{
    public int? ColorId { get; set; }
    public int? FabricTypeId { get; set; }

    public override string Type => ResourceTypes.Fabric;

    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);

        validator.Property("colorId", this.ColorId)
            .NotNull();

        validator.Property("fabricTypeId", this.FabricTypeId)
            .NotNull();

        validator.Property("slotName", this.SlotName)
            .Condition(this.SlotName == null || this.SlotName == SlotNames.Fabric);
    }

    public override void ApplyTo (StoredResource fabric)
    {
        base.ApplyTo(fabric);
        fabric.ColorId = this.ColorId;
        fabric.FabricTypeId = this.FabricTypeId;
    }
}
