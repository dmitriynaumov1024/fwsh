namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class ResourceCreationRequest<TStored> : Request, CreationRequest<TStored>
where TStored: class
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double PricePerUnit { get; set; }
    public int InStock { get; set; }
    public int NormalStock { get; set; }
    public int RefillPeriodDays { get; set; }
    public int SupplierId { get; set; }
    public string ExternalId { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("name", this.Name)
                .NotNull().LengthInRange(2, 30);

        validator.Property("externalId", this.ExternalId)
                .NotNull().LengthInRange(0, 20);

        validator.Property("description", this.Description)
                .NotNull().LengthInRange(1, 200);

        validator.Property("pricePerUnit", this.PricePerUnit)
                .ValueInRange(0.01, 99999);

        validator.Property("inStock", this.InStock)
                .ValueInRange(0, 10000);

        validator.Property("normalStock", this.NormalStock)
                .ValueInRange(1, 10000);

        validator.Property("refillPeriodDays", this.RefillPeriodDays)
                .ValueInRange(2, 365);
    }

    public abstract TStored Create();
}
