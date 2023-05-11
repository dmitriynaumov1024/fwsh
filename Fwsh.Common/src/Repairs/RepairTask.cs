namespace Fwsh.Common;

using System;

public class RepairTask : WorkTask
{
    public int OrderId { get; set; }
    
    public string Description { get; set; }
    public string Role { get; set; }

    public virtual RepairOrder Order { get; set; }
}
