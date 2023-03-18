namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class ResourceUpdateRequest<TStored> : UpdateRequest<TStored>
where TStored : class
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

    public void OnBeforeUpdate<TCount, TResource> (StoredResource<TCount, TResource> stored)
    where TResource : Resource
    {
        stored.NormalStock = this.NormalStock;
        stored.SupplierId = this.SupplierId;
        stored.ExternalId = this.ExternalId;
        stored.RefillPeriodDays = this.RefillPeriodDays;

        if (stored.Item != null) {
            stored.Item.Name = this.Name;
            stored.Item.Description = this.Description;
            stored.Item.PricePerUnit = this.PricePerUnit;
        }
    }
}
