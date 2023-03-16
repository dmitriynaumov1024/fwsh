namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class FabricCreationRequest : CreationRequest<StoredFabric>
{
    protected override void OnValidation (ObjectValidator validator)
    {
        validator.DoNothing();
    }

    public override StoredFabric Create()
    {
        return null;
    }
}
