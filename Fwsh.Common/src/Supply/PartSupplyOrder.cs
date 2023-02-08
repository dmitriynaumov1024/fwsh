namespace Fwsh.Common;

using System;

public class PartSupplyOrder : SupplyOrder<int, Part>
{
    public PartSupplyOrder () : base () { }

    public PartSupplyOrder (StoredPart part) : base (part) { }
}
