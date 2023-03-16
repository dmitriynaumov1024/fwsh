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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Results.Resources;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

// TStoredResult is necessary because of [HttpGet("list")] List
// which is to return list of results but there is inheritance tree
// and System.Text.Json is 'efficiently' omitting properties of derived types
//
public abstract class ResourceController<TCount, TResource, TStored, TStoredResult> : ControllerBase
where TStored : StoredResource<TCount, TResource> 
where TStoredResult : StoredResourceResult<TCount, TResource> 
where TResource : Resource
{
    const int PAGESIZE = 10;

    protected string typeName = typeof(TStored).Name;

    protected FwshDataContext dataContext;
    protected Logger logger;
    protected FwshUser user;

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

    protected abstract TStoredResult CreateMiniResultFrom (TStored item);
    protected abstract TStoredResult CreateFullResultFrom (TStored item);

    [HttpGet("list")]
    public IActionResult List (int page = -1) 
    {
        if (page < 0) {
            return BadRequest ( new BadFieldResult("page") );
        }

        var result = dbQueryableSet.OrderBy(r => r.Id)
            .Paginate(page, PAGESIZE, item => CreateMiniResultFrom(item));

        return Ok (result);
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
        
        return Ok ( CreateFullResultFrom(existingResource) );
    }

    [HttpPost("set-quantity/{id}")]
    public IActionResult SetQuantity (int id, [FromBody] TCount quantity)
    {
        var storedResource = dbSet.Find(id);

        if (storedResource == null) {
            return NotFound ( new BadFieldResult("id") );
        }

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

    public IActionResult OnCreate (CreationRequest<TStored> request)
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
            return BadRequest ( new FailResult(message) );
        }
    }

    public IActionResult OnUpdate (int id, UpdateRequest<TStored> request)
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
