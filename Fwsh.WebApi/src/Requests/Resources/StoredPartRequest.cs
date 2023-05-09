namespace Fwsh.WebApi.Requests.Resources;

using Fwsh.Common;
using Fwsh.WebApi.Validation;

public class StoredPartRequest : ResourceRequest
{
    public override string Type => ResourceTypes.Part;

    protected override void OnValidation (ObjectValidator validator)
    {
        base.OnValidation(validator);
    }
    
    public override void ApplyTo (StoredResource res) 
    {
        base.ApplyTo(res);
        res.Precision = 0;
    }
}
