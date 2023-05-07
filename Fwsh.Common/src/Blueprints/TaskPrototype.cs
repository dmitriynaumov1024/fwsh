namespace Fwsh.Common;

using System;
using System.Collections.Generic;
using System.Linq;

public class TaskPrototype
{
    public int Id { get; set; }
    public int DesignId { get; set; }

    public int Precedence { get; set; }
    public string RoleName { get; set; }
    public int Payment { get; set; }

    public string Description { get; set; }

    public virtual Design Design { get; set; }

    public virtual ICollection<ResourceQuantity> Resources { get; set; }

    public virtual IEnumerable<ResourceQuantity> Parts => 
        Resources.Where(r => r.Item.Type == ResourceTypes.Part);

    public virtual IEnumerable<ResourceQuantity> Materials => 
        Resources.Where(r => r.Item.Type == ResourceTypes.Material);

    public virtual IEnumerable<ResourceQuantity> Fabrics => 
        Resources.Where(r => r.Item.Type == ResourceTypes.Fabric);

    public TaskPrototype()
    {
        this.Resources = new HashSet<ResourceQuantity>();
    }

    public int CalculateResourcePrice ()
    {
        return this.Parts.Sum(part => part.CalculateResourcePrice())
            + this.Materials.Sum(mat => mat.CalculateResourcePrice())
            + this.Fabrics.Sum(fab => fab.CalculateResourcePrice());
    }
}
