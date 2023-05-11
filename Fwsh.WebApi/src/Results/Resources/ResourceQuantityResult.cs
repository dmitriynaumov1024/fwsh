namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class ResourceQuantityResult : Result
{
    public int? ItemId { get; set; }
    public string SlotName { get; set; } = SlotNames.Unknown;
    public string ItemName { get; set; }
    public double ExpectQuantity { get; set; }
    public double ActualQuantity { get; set; }

    public StoredResourceResult Item { get; set; }

    public ResourceQuantityResult () { }

    public ResourceQuantityResult (ResourceQuantity res) 
    {
        this.ItemId = res.ItemId;
        this.SlotName = res.SlotName;
        this.ItemName = res.ItemName;
        this.ExpectQuantity = res.ExpectQuantity;
        this.ActualQuantity = res.ActualQuantity;

        if (res.Item is Resource item) {
            this.Item = new StoredResourceResult(item);
        }
    }
}
