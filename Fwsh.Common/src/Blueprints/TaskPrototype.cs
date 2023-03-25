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
    public string InstructionUrl { get; set; }

    public virtual Design Design { get; set; }

    public virtual ICollection<TaskPart> Parts { get; set; }
    public virtual ICollection<TaskMaterial> Materials { get; set; }
    public virtual ICollection<TaskFabric> Fabrics { get; set; }

    public TaskPrototype()
    {
        this.Parts = new HashSet<TaskPart>();
        this.Materials = new HashSet<TaskMaterial>();
        this.Fabrics = new HashSet<TaskFabric>();
    }

    public int CalculateResourcePrice ()
    {
        return this.Parts.Sum(part => part.CalculateResourcePrice())
            + this.Materials.Sum(mat => mat.CalculateResourcePrice())
            + this.Fabrics.Sum(fab => fab.CalculateResourcePrice());
    }
}
