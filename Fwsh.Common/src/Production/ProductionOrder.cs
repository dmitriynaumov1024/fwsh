namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class ProductionOrder : Order
{
    public int DesignId { get; set; }
    public int DecorMaterialId { get; set; }
    public int FabricId { get; set; }

    public int PricePerOne { get; set; }
    public int Quantity { get; set; } 
    public int Prepayment { get; set; }

    public virtual Design Design { get; set; }
    public virtual Fabric Fabric { get; set; }
    public virtual Material DecorMaterial { get; set; }
    public virtual ICollection<ProductionNotification> Notifications { get; set; }
    public virtual ICollection<ProductionTask> Tasks { get; set; }

    public int PriceTotal => this.PricePerOne * this.Quantity;

    public ProductionOrder() : base()
    {
        this.Notifications = new HashSet<ProductionNotification>();
        this.Tasks = new HashSet<ProductionTask>();
    }
}
