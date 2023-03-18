namespace Fwsh.WebApi.Requests.Resources;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;
using System.Text.RegularExpressions;

public class FabricUpdateRequest : ResourceUpdateRequest<StoredFabric>
{
    public int ColorId { get; set; }
    public int FabricTypeId { get; set; }

    public Color Color { get; set; }
    public FabricType FabricType { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);

        if (this.Color != null) {
            validator.Property("color.name", this.Color.Name)
                    .NotNull().LengthInRange(3, 24);

            validator.Property("color.rgbCode", this.Color.RgbCode)
                    .NotNull().Match(RgbCodeRegex);
        }

        if (this.FabricType != null) {
            validator.Property("fabricType.name", this.FabricType.Name)
                    .NotNull().LengthInRange(3, 40);

            validator.Property("fabricType.description", this.FabricType.Description)
                    .NotNull().LengthInRange(3, 200);
        }
    }

    public override void ApplyTo (StoredFabric stored)
    {
        base.OnBeforeUpdate(stored);

        stored.Quantity = this.InStock;
        
        if (stored.Item is Fabric fabric) {
            fabric.ColorId = this.ColorId;
            fabric.FabricTypeId = this.FabricTypeId;

            if (this.Color != null) fabric.Color = new Color() {
                Name = this.Color.Name,
                RgbCode = this.Color.RgbCode
            };

            if (this.FabricType != null) fabric.FabricType = new FabricType() {
                Name = this.FabricType.Name,
                Description = this.FabricType.Description
            };
        }
    }

    static Regex RgbCodeRegex = new Regex(@"^\#[0-9a-fA-F]{6}$");
}
