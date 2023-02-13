namespace Fwsh.Common;

using System;

public abstract class TaskResourceUsage<TCount, TResource> : ResourceQuantity<TCount, TResource>
{
    public int Id { get; set; }
    public int TaskId { get; set; }
}
