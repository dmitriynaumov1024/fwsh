namespace Fwsh.Common;

using System;

public class MaterialSupplyOrder : SupplyOrder<double, Material>
{
    public MaterialSupplyOrder () : base () { }

    public MaterialSupplyOrder (StoredMaterial mat) : base (mat) { }
}
