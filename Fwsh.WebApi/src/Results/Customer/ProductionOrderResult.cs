namespace Fwsh.WebApi.Results.Customer;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;
using Fwsh.WebApi.Results.Common;
using Fwsh.WebApi.Results.Catalog;

public class ProductionOrderResult : MiniProductionOrderResult
{
    public FabricResult Fabric { get; set; }
    public DecorMaterialResult DecorMaterial { get; set; }
    public CustomerProfileResult Customer { get; set; }
    public ICollection<NotificationResult> Notifications { get; set; }

    public ProductionOrderResult (ProductionOrder order) : base(order)
    {
        this.Fabric = new FabricResult(order.Fabric);
        this.DecorMaterial = new DecorMaterialResult(order.DecorMaterial);
        this.Customer = new CustomerProfileResult(order.Customer);
        this.Notifications = order.Notifications
            .Select(n => new NotificationResult(n)).ToList();

    }
}
