namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class ProductionTask : WorkTask
{
    public int PrototypeId { get; set; }
    public int OrderId { get; set; }

    public virtual TaskPrototype Prototype { get; set; }
    public virtual ProductionOrder Order { get; set; }
}
