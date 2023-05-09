namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

using Fwsh.Common;

public class ProdOrderResult : OrderResult, IResultBuilder<ProdOrderResult>
{
    private ProdOrder order;

    public int Quantity { get; set; }
    public int PricePerOne { get; set; }

    public int DesignId { get; set; }
    public int FabricId { get; set; }
    public int? DecorId { get; set; }

    public DesignResult Design { get; set; }
    public ResourceResult Fabric { get; set; }
    public ResourceResult Decor { get; set; }

    public ProdOrderResult () { }

    public ProdOrderResult (ProdOrder order)
    {
        this.order = order;
    }
    
    public ProdOrderResult Mini () 
    {
        var result = new ProdOrderResult() {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            StartedAt = order.StartedAt,
            FinishedAt = order.FinishedAt,
            ReceivedAt = order.ReceivedAt,
            Quantity = order.Quantity,
            PricePerOne = order.PricePerOne,
            Price = order.Price,
            DesignId = order.DesignId,
            FabricId = order.FabricId,
            DecorId = order.DecorId
        };

        if (order.Design != null) 
            result.Design = new DesignResult(order.Design).Mini();

        return result;
    }

    public ProdOrderResult ForCustomer ()
    {
        var result = Mini();

        if (order.Fabric != null) 
            result.Fabric = new ResourceResult(order.Fabric);
        
        if (order.Decor != null)
            result.Decor = new ResourceResult(order.Decor);
        
        result.Notifications = order.Notifications
            .OrderBy(n => n.Id)
            .Select(n => new NotificationResult(n))
            .ToList();
        
        return result;
    }

    public ProdOrderResult ForWorker ()
    {
        var result = ForCustomer();
        return result;
    }

    public ProdOrderResult ForManager ()
    {
        var result = ForWorker();
        result.Customer ??= new CustomerResult(order.Customer);
        return result;
    }
}
