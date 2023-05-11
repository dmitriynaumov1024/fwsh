namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

public class ResourceQuantityResult : Result
{
    public int? ItemId { get; set; }
    public string SlotName { get; set; } = SlotNames.Unknown;
    public string ItemName { get; set; }
    public double Quantity { get; set; }
    
    public StoredResourceResult Item { get; set; }

    public ResourceQuantityResult () { }

    public ResourceQuantityResult (ResourceQuantity res) 
    {
        this.ItemId = res.ItemId;
        this.SlotName = res.SlotName;
        this.ItemName = res.ItemName;
        this.Quantity = res.Quantity;

        if (res.Item is Resource item) {
            this.Item = new StoredResourceResult(item);
        }
    }
}
