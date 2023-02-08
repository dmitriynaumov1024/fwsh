namespace Fwsh.Common;

using System;

public class FabricSupplyOrder : SupplyOrder<double, Fabric>
{
    public FabricSupplyOrder () : base () { }

    public FabricSupplyOrder (StoredFabric fabric) : base (fabric) { }
}
