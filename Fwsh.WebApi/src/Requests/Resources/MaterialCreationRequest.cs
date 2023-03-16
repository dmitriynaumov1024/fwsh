namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;
using System.Text.RegularExpressions;

public class MaterialCreationRequest : ResourceCreationRequest<StoredMaterial>
{
    public string MeasureUnit { get; set; }
    public bool IsDecorative { get; set; }
    public int? ColorId { get; set; }

    public Color Color { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);

        validator.Property("measureUnit", this.MeasureUnit)
                .Condition(MeasureUnits.Contains(this.MeasureUnit));

        validator.Property("isDecorative", this.IsDecorative)
                .Condition(this.IsDecorative == (this.Color != null || this.ColorId != null));

        if (this.Color != null) 
        {
            validator.Property("color.name", this.Color.Name)
                    .NotNull().LengthInRange(3, 24);

            validator.Property("color.rgbCode", this.Color.RgbCode)
                    .NotNull().Match(RgbCodeRegex);
        }
    }
    
    public override StoredMaterial Create()
    {
        return new StoredMaterial() {
            Quantity = this.InStock,
            NormalStock = this.NormalStock,
            ExternalId = this.ExternalId,
            SupplierId = this.SupplierId,
            RefillPeriodDays = this.RefillPeriodDays,
            Item = new Material() {
                Name = this.Name,
                Description = this.Description,
                PricePerUnit = this.PricePerUnit,
                MeasureUnit = this.MeasureUnit,
                ColorId = this.ColorId,
                Color = (this.Color != null) ? new Color() {
                    Name = this.Color.Name,
                    RgbCode = this.Color.RgbCode
                } : null,
                PhotoUrl = ""
            }
        };
    }

    static Regex RgbCodeRegex = new Regex(@"^\#[0-9a-fA-F]{6}$");
}
