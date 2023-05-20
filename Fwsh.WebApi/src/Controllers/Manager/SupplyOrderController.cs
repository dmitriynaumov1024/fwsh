namespace Fwsh.WebApi.Controllers.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Requests.Manager;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("/manager/orders/supply")]
public class SupplyOrderController : FwshController
{
    const int PAGESIZE = 10;
    const int MAX_SIZE = 100;

    public SupplyOrderController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1, int? resource = null) 
    {
        if (page < 0) return NotFound(new BadFieldResult("page"));

        IQueryable<SupplyOrder> orders = dataContext.SupplyOrders
            .Include(o => o.Item.Stored)
            .Include(o => o.Supplier)
            .Where(o => o.IsActive);
        
        if (resource is int resourceId) 
            orders = orders.Where(o => o.ItemId == resourceId);

        return Ok ( orders.OrderByDescending(order => order.Id)
            .Paginate(page, PAGESIZE, order => new SupplyOrderResult(order))
        );
    }

    [HttpGet("archive")]
    public IActionResult Archive (int page = -1, int? resource = null)
    {
        if (page < 0) return NotFound(new BadFieldResult("page"));

        IQueryable<SupplyOrder> orders = dataContext.SupplyOrders
            .Include(o => o.Item)
            .Include(o => o.Supplier)
            .Where(o => !o.IsActive);
        
        if (resource is int resourceId) 
            orders = orders.Where(o => o.ItemId == resourceId);

        return Ok ( orders.OrderByDescending(order => order.Id)
            .Paginate(page, PAGESIZE, order => new SupplyOrderResult(order))
        );
    }

    [HttpGet("view/{id}")]
    public IActionResult Get (int id) 
    {
        SupplyOrder order = dataContext.SupplyOrders
            .Include(o => o.Item.Stored)
            .Include(o => o.Supplier)
            .FirstOrDefault(o => o.Id == id);

        if (order == null) 
            return NotFound(new BadFieldResult("id"));

        return Ok (new SupplyOrderResult(order));
    }

    [HttpPost("create")]
    public IActionResult Create (SupplyOrderRequest request)
    {
        if (request.Validate().State.HasBadFields || !request.State.IsValid) {
            return BadRequest (new BadFieldResult(request.State.BadFields));
        }

        try {
            SupplyOrder order = request.Create();
            order.IsActive = true;
            dataContext.SupplyOrders.Add(order);
            dataContext.SaveChanges();
            return Ok(new CreationResult(order.Id, "Successfully created Supply order"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not create Supply order"));
        }
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, SupplyOrderRequest request)
    {
        if (request.Validate().State.HasBadFields || !request.State.IsValid) {
            return BadRequest (new BadFieldResult(request.State.BadFields));
        }

        SupplyOrder order = dataContext.SupplyOrders
            .FirstOrDefault(order => order.Id == id);

        if (order == null || !order.IsActive) 
            return NotFound(new BadFieldResult("id"));

        try {
            request.ApplyTo(order);
            dataContext.SupplyOrders.Update(order);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully updated Supply order"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not update Supply order"));
        }
    }

    [HttpPost("confirm-submit/{id}")]
    public IActionResult ConfirmSubmit (int id)
    {
        SupplyOrder order = dataContext.SupplyOrders
            .Include(order => order.Item.Stored)
            .FirstOrDefault(order => order.Id == id);

        if (order == null || order.Status != OrderStatus.Unknown) 
            return NotFound(new BadFieldResult("id"));

        try {
            order.Status = OrderStatus.Submitted;
            order.IsActive = true;
            order.Item.Stored.SupplyOrderCount += 1;
            dataContext.SupplyOrders.Update(order);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully submitted Supply order"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not update Supply order"));
        }
    }

    [HttpPost("confirm-receive/{id}")]
    public IActionResult ConfirmReceive (int id)
    {
        SupplyOrder order = dataContext.SupplyOrders
            .Include(order => order.Item.Stored)
            .FirstOrDefault(order => order.Id == id);

        if (order == null || order.Status != OrderStatus.Submitted) 
            return NotFound(new BadFieldResult("id"));

        try {
            order.IsActive = false;

            if (order.ActualQuantity > 0) {
                order.Status = OrderStatus.Received;
                order.ReceivedAt = DateTime.UtcNow;
                var stored = order.Item.Stored;
                double newPricePerUnit = (stored.InStock * order.Item.PricePerUnit 
                    + order.ActualQuantity * order.ActualPricePerUnit) 
                    / (stored.InStock + order.ActualQuantity);
                order.Item.PricePerUnit = newPricePerUnit;
                stored.InStock += order.ActualQuantity;
                stored.LastRefilledAt = DateTime.UtcNow;
            } 
            else {
                order.Status = OrderStatus.Impossible;
            }

            if (order.Item.Stored.SupplyOrderCount > 0) {
                order.Item.Stored.SupplyOrderCount -= 1;
            }

            dataContext.SupplyOrders.Update(order);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully received Supply order"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not update Supply order"));
        }
    }
}
