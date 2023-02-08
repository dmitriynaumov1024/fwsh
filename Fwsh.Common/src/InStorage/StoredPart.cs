namespace Fwsh.Common;

using System;

public class StoredPart : StoredResource<int, Part>
{
    public override int InStock => this.Quantity;
}
