namespace Fwsh.WebApi.Requests.Manager;

using System;
using System.Text.RegularExpressions;
using Fwsh.Common;
using Fwsh.WebApi.Validation;

public class DesignRequest: Request, 
    CreationRequest<Design>, UpdateRequest<Design>
{
    public string Type { get; set; }
    public string NameKey { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool IsVisible { get; set; }
    public bool IsTransformable { get; set; }
    public Dimensions DimCompact { get; set; }
    public Dimensions DimExpanded { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("type", this.Type)
                .NotNull().Condition(FurnitureTypes.Contains(this.Type));

        validator.Property("nameKey", this.NameKey)
                .NotNull().Match(NameKeyRegex);

        validator.Property("displayName", this.DisplayName)
                .NotNull().Match(DisplayNameRegex);

        validator.Property("description", this.Description) 
                .NotNull().LengthInRange(100, 10000);

        validator.Property("dimCompact", this.DimCompact)
                .NotNull();

        validator.Property("dimExpanded", this.DimExpanded)
                .Condition((! this.IsTransformable) || (this.DimExpanded is Dimensions));
    }

    public Design Create() 
    {
        var design = new Design();
        design.NameKey = this.NameKey;
        this.ApplyTo(design);
        return design;
    } 

    public void ApplyTo (Design design)
    {
        design.Type = this.Type;
        design.DisplayName = this.DisplayName;
        design.Description = this.Description;
        design.IsVisible = this.IsVisible;
        design.DimCompact = this.DimCompact;
        design.DimExpanded = this.DimExpanded;
    }

    static Regex NameKeyRegex = new Regex(@"^[a-z0-9\-]{3,24}$");
    static Regex DisplayNameRegex = new Regex(@"^[A-Za-z0-9\s\-]{3,24}$");
}
