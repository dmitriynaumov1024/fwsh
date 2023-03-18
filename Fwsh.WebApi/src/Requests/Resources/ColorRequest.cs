namespace Fwsh.WebApi.Requests.Resources;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Validation;
using System.Text.RegularExpressions;

public class ColorRequest : Request
{
    public string Name { get; set; }
    public string RgbCode { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("name", this.Name)
                .NotNull().LengthInRange(3, 24);

        validator.Property("rgbCode", this.RgbCode)
                .NotNull().Match(RgbCodeRegex);
    }

    public Color Create()
    {
        return new Color() {
            Name = this.Name,
            RgbCode = this.RgbCode
        };
    }

    public void ApplyTo (Color color)
    {
        color.Name = this.Name;
        color.RgbCode = this.RgbCode;
    }

    static Regex RgbCodeRegex = new Regex(@"^\#[0-9a-fA-F]{6}$");
}
