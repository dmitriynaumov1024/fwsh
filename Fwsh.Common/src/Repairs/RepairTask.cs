namespace Fwsh.Common;

using System;

public class RepairTask : WorkTask
{
    public int OrderId { get; set; }
    
    public string RoleName { get; set; }
    public int Payment { get; set; }

    public virtual RepairOrder Order { get; set; }
}
