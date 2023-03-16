namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;
using Fwsh.Common;
using Fwsh.WebApi.Results.Catalog;

public class StoredFabricResult : StoredResourceResult<double, Fabric>
{
    public FabricResult Item { get; set; }

    public StoredFabricResult () { }

    public StoredFabricResult (StoredFabric resource, bool full = false) 
    : base (resource, full) 
    { 

    }

    public static StoredFabricResult Mini (StoredFabric resource)
    {
        return new StoredFabricResult(resource) {
            Item = new FabricResult() {
                Name = resource.Item.Name,
                PricePerUnit = resource.Item.PricePerUnit
            }
        };
    }

    public static StoredFabricResult Full (StoredFabric resource) 
    {
        return new StoredFabricResult(resource, true) {
            Item = new FabricResult(resource.Item)
        };
    }
}
