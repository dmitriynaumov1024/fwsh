namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class MaterialCreationRequest : CreationRequest<StoredMaterial>
{
    protected override void OnValidation (ObjectValidator validator)
    {
        validator.DoNothing();
    }
    
    public override StoredMaterial Create()
    {
        return null;
    }
}
