namespace Fwsh.WebApi.Requests.Customer;

using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Validation;

public class ProductionOrderCreationRequest : Request
{
    public int Quantity { get; set; }
    public int DesignId { get; set; }
    public int FabricId { get; set; }
    public int DecorMaterialId { get; set; }

    protected override void OnValidation (ObjectValidator validator)
    {
        validator.Property("quantity", this.Quantity).ValueInRange(1, 100);
    }
}
