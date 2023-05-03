namespace Fwsh.WebApi.Controllers.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Requests.Resources;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

// TStoredResult is necessary because of [HttpGet("list")] List
// which is to return list of results but there is inheritance tree
// and System.Text.Json is 'efficiently' omitting properties of derived types
//
public abstract class ResourceController<TCount, TResource, TStored, TStoredResult> : FwshController
where TStored : StoredResource<TCount, TResource> 
where TStoredResult : StoredResourceResult<TCount, TResource> 
where TResource : Resource
{
    const int PAGESIZE = 10;

    protected string typeName = typeof(TStored).Name;

    protected abstract DbSet<TStored> dbSet { get; }
    protected abstract IQueryable<TStored> dbQueryableSet { get; }

    protected virtual bool canCreate => user.ConfirmedRole >= UserRole.Manager;
    protected virtual bool canUpdate => user.ConfirmedRole >= UserRole.Manager;

    public ResourceController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    protected abstract IResultBuilder<TStoredResult> ResultBuilder (TStored item);

    [HttpGet("list")]
    public IActionResult List (int page = -1, bool reverse = false) 
    {
        if (page < 0) {
            return BadRequest ( new BadFieldResult("page") );
        }

        var resources = reverse ? 
            dbQueryableSet.OrderByDescending(r => r.Id) : 
            dbQueryableSet.OrderBy(r => r.Id);

        return Ok (
            resources.Paginate(page, PAGESIZE, item => ResultBuilder(item).Mini())
        );
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id) 
    {
        var existingResource = dbQueryableSet
            .Where(item => item.Id == id)
            .FirstOrDefault();

        if (existingResource == null) {
            return NotFound ( new BadFieldResult("id") );
        } 
        
        return Ok ( ResultBuilder(existingResource).For(user) );
    }

    protected IActionResult OnSetQuantity (int id, TStored storedResource, TCount quantity)
    {
        try {
            storedResource.Quantity = quantity;
            storedResource.LastCheckedAt = DateTime.UtcNow;
            dbSet.Update(storedResource);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully updated stock quantity"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return BadRequest(new FailResult("Can not update stock quantity"));
        }
    }

    protected IActionResult OnCreate (ResourceCreationRequest<TStored> request)
    {
        if (! canCreate) {
            return BadRequest(new FailResult("Not enough rights to create"));
        }

        try {
            if (request.Validate().State.HasBadFields) {
                return BadRequest(new BadFieldResult(request.State.BadFields));
            }
            if (! request.State.IsValid) {
                return BadRequest(new FailResult(request.State.Message ?? 
                    "Something went wrong while validating creation request"));
            }

            TStored newResource = request.Create();

            dbSet.Add(newResource);
            dataContext.SaveChanges();

            string message = $"Successfully created {typeName}";
            return Ok ( new CreationResult(newResource.Id, message) );
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            string message = $"Something went wrong while trying to create {typeName}";
            return ServerError ( new FailResult(message) );
        }
    }

    protected IActionResult OnUpdate (int id, ResourceUpdateRequest<TStored> request)
    {
        if (! canUpdate) {
            return BadRequest(new FailResult("Not enough rights to create"));
        }
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? 
                "Something went wrong while validating creation request"));
        }

        TStored existingResource = dbQueryableSet
            .Where(item => item.Id == id)
            .FirstOrDefault();
        
        if (existingResource == null) {
            return BadRequest(new BadFieldResult("id"));
        }

        request.ApplyTo(existingResource);

        try {
            dbSet.Update(existingResource);
            dataContext.SaveChanges();
            string message = $"Successfully updated {typeName} with id={existingResource.Id}";
            return Ok ( new SuccessResult(message) );
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            string message = $"Something went wrong while trying to update {typeName}";
            return BadRequest ( new FailResult() );
        }
    }

}
