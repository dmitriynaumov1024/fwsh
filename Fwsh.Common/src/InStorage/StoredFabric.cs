namespace Fwsh.Common;

using System;

public class StoredFabric : StoredResource<double, Fabric>
{
    public override int InStock => (int)Math.Round(this.Quantity);
}
