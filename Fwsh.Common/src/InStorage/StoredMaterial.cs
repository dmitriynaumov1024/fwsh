namespace Fwsh.Common;

using System;

public class StoredMaterial : StoredResource<double, Material>
{
    public override int InStock => (int)Math.Round(this.Quantity);
}
