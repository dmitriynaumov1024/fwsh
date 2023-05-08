namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class ProdTask : WorkTask
{
    public int PrototypeId { get; set; }
    public int? FurnitureId { get; set; }

    public virtual TaskPrototype Prototype { get; set; }
    public virtual ProdFurniture Furniture { get; set; }
}
