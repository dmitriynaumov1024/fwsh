namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public class ProdOrder : Order
{
    public int DesignId { get; set; }
    public int FabricId { get; set; }
    public int? DecorId { get; set; }

    public int PricePerOne { get; set; }
    public int Quantity { get; set; } 
    public int Prepayment { get; set; }

    public virtual Design Design { get; set; }
    public virtual Resource Fabric { get; set; }
    public virtual Resource Decor { get; set; }

    public virtual ICollection<ProdFurniture> Furnitures { get; set; }
    public virtual ICollection<ProdNotification> Notifications { get; set; }

    public ProdOrder() : base()
    {
        this.Notifications = new HashSet<ProdNotification>();
        this.Furnitures = new HashSet<ProdFurniture>();
    }
}
