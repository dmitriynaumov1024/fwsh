namespace Fwsh.Common;

public abstract class ResourceQuantity<TCount, TResource>
{
    public TCount Quantity { get; set; }
    public virtual TResource Item { get; set; }

    public abstract int CalculateResourcePrice();
}
