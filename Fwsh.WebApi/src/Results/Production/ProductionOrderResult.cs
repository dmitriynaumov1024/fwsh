namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class ProductionOrderResult : OrderResult, IResultBuilder<ProductionOrderResult>
{
    private ProductionOrder order;

    public int Quantity { get; set; }
    public int PricePerOne { get; set; }
    public int PriceTotal { get; set; }

    public int DesignId { get; set; }
    public int FabricId { get; set; }
    public int DecorMaterialId { get; set; }

    public DesignResult Design { get; set; }
    public FabricResult Fabric { get; set; }
    public MaterialResult DecorMaterial { get; set; }

    

    public ProductionOrderResult () { }

    public ProductionOrderResult (ProductionOrder order)
    {
        this.order = order;
    }
    
    public ProductionOrderResult Mini () 
    {
        return new ProductionOrderResult() {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            StartedAt = order.StartedAt,
            FinishedAt = order.FinishedAt,
            ReceivedAt = order.ReceivedAt,
            Quantity = order.Quantity,
            PricePerOne = order.PricePerOne,
            PriceTotal = order.PriceTotal,
            DesignId = order.DesignId,
            FabricId = order.FabricId,
            DecorMaterialId = order.DecorMaterialId
        };
    }

    public ProductionOrderResult ForCustomer ()
    {
        var result = Mini();

        result.Design = new DesignResult(order.Design).Mini();
        result.Fabric = new FabricResult(order.Fabric);
        result.DecorMaterial = new MaterialResult(order.DecorMaterial);
        
        result.Notifications = order.Notifications
            .OrderBy(n => n.Id)
            .Select(n => new NotificationResult(n))
            .ToList();
        
        return result;
    }

    public ProductionOrderResult ForWorker ()
    {
        var result = ForCustomer();
        return result;
    }

    public ProductionOrderResult ForManager ()
    {
        var result = ForWorker();
        result.Customer ??= new CustomerResult(order.Customer);
        return result;
    }
}
