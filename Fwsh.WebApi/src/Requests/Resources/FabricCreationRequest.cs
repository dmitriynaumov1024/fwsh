namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class FabricCreationRequest : ResourceCreationRequest<StoredFabric>
{
    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);
    }

    public override StoredFabric Create()
    {
        return null;
    }
}
