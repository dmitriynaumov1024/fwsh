namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public class PartCreationRequest : ResourceCreationRequest<StoredPart>
{
    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);
    }
    
    public override StoredPart Create()
    {
        return new StoredPart() {
            Quantity = this.InStock,
            NormalStock = this.NormalStock,
            ExternalId = this.ExternalId,
            SupplierId = this.SupplierId,
            RefillPeriodDays = this.RefillPeriodDays,
            Item = new Part() {
                Name = this.Name,
                Description = this.Description,
                PricePerUnit = this.PricePerUnit
            }
        };
    }
}
