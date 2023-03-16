namespace Fwsh.WebApi.Requests.Resources;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;
using System.Text.RegularExpressions;

public class FabricCreationRequest : ResourceCreationRequest<StoredFabric>
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

    public override StoredFabric Create()
    {
        return new StoredFabric() {
            Quantity = this.InStock,
            NormalStock = this.NormalStock,
            ExternalId = this.ExternalId,
            SupplierId = this.SupplierId,
            RefillPeriodDays = this.RefillPeriodDays,
            Item = new Fabric() {
                Name = this.Name,
                Description = this.Description,
                PricePerUnit = this.PricePerUnit,
                ColorId = this.ColorId,
                FabricTypeId = this.FabricTypeId,
                Color = (this.Color != null) ? new Color() {
                    Name = this.Color.Name,
                    RgbCode = this.Color.RgbCode
                } : null,
                FabricType = (this.FabricType != null) ? new FabricType() {
                    Name = this.FabricType.Name,
                    Description = this.FabricType.Description
                } : null
            }
        };
    }

    static Regex RgbCodeRegex = new Regex(@"^\#[0-9a-fA-F]{6}$");
}
