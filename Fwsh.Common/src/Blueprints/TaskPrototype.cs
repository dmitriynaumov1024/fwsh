namespace Fwsh.Common;

using System;
using System.Collections.Generic;
using System.Linq;

public class TaskPrototype
{
    public int Id { get; set; }
    public int DesignId { get; set; }

    public int Precedence { get; set; }
    public string Role { get; set; }
    public int Payment { get; set; }

    public string Description { get; set; }

    public virtual Design Design { get; set; }

    public virtual ICollection<ResourceQuantity> Resources { get; set; }

    public virtual IEnumerable<ResourceQuantity> Parts => 
        this.Resources.Where(r => r.Item != null && r.Item.Type == ResourceTypes.Part);

    public virtual IEnumerable<ResourceQuantity> Materials => 
        this.Resources.Where(r => r.Item != null && r.Item.Type == ResourceTypes.Material || r.SlotName == SlotNames.Decor);

    public virtual IEnumerable<ResourceQuantity> Fabrics => 
        this.Resources.Where(r => r.Item != null && r.Item.Type == ResourceTypes.Fabric || r.SlotName == SlotNames.Fabric);

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
