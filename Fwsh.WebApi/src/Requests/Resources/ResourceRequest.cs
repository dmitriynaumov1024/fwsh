namespace Fwsh.WebApi.Requests.Resources;

using System;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public class ResourceRequest : Request, CreationRequest<StoredResource>, UpdateRequest<StoredResource>
{
    public string Name { get; set; }
    public string SlotName { get; set; }
    public string Description { get; set; }
    public string MeasureUnit { get; set; }
    public double PricePerUnit { get; set; }
    public int Precision { get; set; }
    public double InStock { get; set; }
    public double NormalStock { get; set; }
    public int RefillPeriodDays { get; set; }
    public int? SupplierId { get; set; }
    public string ExternalId { get; set; }

    public virtual string Type => ResourceTypes.Resource;

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("name", this.Name)
                .NotNull().LengthInRange(2, 30);

        validator.Property("slotName", this.SlotName)
                .Condition(SlotNames.Contains(this.SlotName));

        validator.Property("externalId", this.ExternalId)
                .NotNull().LengthInRange(0, 20);

        validator.Property("description", this.Description)
                .NotNull().LengthInRange(1, 200);

        validator.Property("measureUnit", this.MeasureUnit)
                .NotNull().Condition(MeasureUnits.Contains(this.MeasureUnit));

        validator.Property("precision", this.Precision)
                .ValueInRange(0, 6);

        validator.Property("pricePerUnit", this.PricePerUnit)
                .ValueInRange(0.01, 99999);

        validator.Property("inStock", this.InStock)
                .ValueInRange(0, 10000);

        validator.Property("normalStock", this.NormalStock)
                .ValueInRange(1, 10000);

        validator.Property("refillPeriodDays", this.RefillPeriodDays)
                .ValueInRange(2, 365);
    }

    public virtual StoredResource Create ()
    {
        var result = new StoredResource();
        this.ApplyTo(result);
        return result;
    }

    public virtual void ApplyTo (StoredResource res) 
    {
        res.Type = this.Type;
        res.SlotName = this.SlotName;
        res.Name = this.Name;
        res.Description = this.Description;
        res.MeasureUnit = this.MeasureUnit;
        res.Precision = this.Precision;
        res.PricePerUnit = this.PricePerUnit;
        res.InStock = Math.Round(this.InStock, this.Precision);
        res.NormalStock = Math.Round(this.NormalStock, this.Precision);
        res.RefillPeriodDays = this.RefillPeriodDays;
        res.SupplierId = this.SupplierId;
        res.ExternalId = this.ExternalId;
    }
}
