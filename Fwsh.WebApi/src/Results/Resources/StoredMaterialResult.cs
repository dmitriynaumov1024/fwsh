namespace Fwsh.WebApi.Results.Resources;

using System;
using System.Collections.Generic;
using Fwsh.Common;
using Fwsh.WebApi.Results.Catalog;

public class StoredMaterialResult : StoredResourceResult<double, Material>
{
    public MaterialResult Item { get; set; }

    public StoredMaterialResult () { }

    public StoredMaterialResult (StoredMaterial resource, bool full = false) 
    : base (resource, full) 
    { 

    }

    public static StoredMaterialResult Mini (StoredMaterial resource)
    {
        return new StoredMaterialResult(resource) {
            Item = new MaterialResult() {
                Name = resource.Item.Name,
                PricePerUnit = resource.Item.PricePerUnit
            }
        };
    }

    public static StoredMaterialResult Full (StoredMaterial resource) 
    {
        return new StoredMaterialResult(resource, true) {
            Item = new MaterialResult(resource.Item)
        };
    }
}
