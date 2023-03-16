namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public class MaterialCreationRequest : ResourceCreationRequest<StoredMaterial>
{
    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);
    }
    
    public override StoredMaterial Create()
    {
        return null;
    }
}
