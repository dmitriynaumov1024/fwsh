namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;
using System.Text.RegularExpressions;

public class MaterialUpdateRequest : ResourceUpdateRequest<StoredMaterial>
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
    
    public override void ApplyTo (StoredMaterial stored)
    {
        base.OnBeforeUpdate(stored);

        stored.Quantity = this.InStock;
        
        if (stored.Item is Material mat) {
            mat.ColorId = this.ColorId;
            mat.MeasureUnit = this.MeasureUnit;
            mat.IsDecorative = this.IsDecorative;

            if (this.Color != null) mat.Color = new Color() {
                Name = this.Color.Name,
                RgbCode = this.Color.RgbCode
            };
        }
    }

    static Regex RgbCodeRegex = new Regex(@"^\#[0-9a-fA-F]{6}$");
}
