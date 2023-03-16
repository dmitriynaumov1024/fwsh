namespace Fwsh.WebApi.Results.Resources;

using System;
using Fwsh.Common;

public class StoredPartResult : StoredResourceResult<int, Part>
{
    public PartResult Item { get; set; }

    public StoredPartResult () { }

    public StoredPartResult (StoredPart resource, bool full = false) 
    : base (resource, full) 
    { 

    }

    public static StoredPartResult Mini (StoredPart resource)
    {
        return new StoredPartResult(resource) {
            Item = new PartResult() {
                Name = resource.Item.Name,
                PricePerUnit = resource.Item.PricePerUnit
            }
        };
    }

    public static StoredPartResult Full (StoredPart resource) 
    {
        return new StoredPartResult(resource, true) {
            Item = new PartResult(resource.Item)
        };
    }
}
