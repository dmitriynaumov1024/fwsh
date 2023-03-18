namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public class PartUpdateRequest : ResourceUpdateRequest<StoredPart>
{
    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);
    }
    
    public override void ApplyTo (StoredPart stored)
    {
        base.OnBeforeUpdate(stored);
        
        stored.Quantity = this.InStock;
    }
}
