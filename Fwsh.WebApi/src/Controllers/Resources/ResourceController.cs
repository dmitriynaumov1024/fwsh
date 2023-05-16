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
using Fwsh.WebApi.FileStorage;
using Fwsh.WebApi.Utils;

public abstract class ResourceController : FwshController
{
    protected FileStorageProvider storage;

    const int PAGESIZE = 10;

    protected virtual string typeName => ResourceTypes.Resource;

    protected virtual DbSet<Resource> dbSet => dataContext.Resources; 
    protected abstract IQueryable<Resource> dbQueryableSet { get; }

    protected virtual bool canCreate => user.ConfirmedRole >= UserRole.Manager;
    protected virtual bool canUpdate => user.ConfirmedRole >= UserRole.Manager;

    public ResourceController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1, bool reverse = false, string query = null) 
    {
        if (page < 0) {
            return BadRequest ( new BadFieldResult("page") );
        }

        IQueryable<Resource> resources = dbQueryableSet;

        if (query != null) { 
            query = query.ToLower();
            resources = resources.Where(r => r.Name.ToLower().Contains(query));
        }

        resources = reverse ? 
            resources.OrderByDescending(r => r.Id) : 
            resources.OrderBy(r => r.Id);

        return Ok (
            resources.Paginate(page, PAGESIZE, item => new StoredResourceResult(item))
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
        
        return Ok ( new StoredResourceResult(existingResource) );
    }

    protected IActionResult OnCreate (ResourceRequest request)
    {
        if (! canCreate) {
            return BadRequest(new FailResult("Not enough rights to create"));
        }

        try {
            if (request.Validate().State.HasBadFields) {
                return BadRequest(new BadFieldResult(request.State.BadFields));
            }

            Resource newResource = request.Create();

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

    protected IActionResult OnUpdate (int id, ResourceRequest request)
    {
        if (! canUpdate) {
            return BadRequest(new FailResult("Not enough rights to create"));
        }
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }

        Resource resource = dbQueryableSet.FirstOrDefault(r => r.Id == id);
        
        if (resource == null) {
            return BadRequest(new BadFieldResult("id"));
        }

        request.ApplyTo(resource);

        try {
            dbSet.Update(resource);
            dataContext.SaveChanges();
            string message = $"Successfully updated {typeName} with id={resource.Id}";
            return Ok ( new SuccessResult(message) );
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            string message = $"Something went wrong while trying to update {typeName}";
            return BadRequest ( new FailResult() );
        }
    }

    [HttpPost("attach-photo/{id}")]
    public IActionResult AttachPhoto (int id)
    {
        Resource res = dataContext.Resources.Find(id);
        
        if (res == null) 
            return BadRequest (new BadFieldResult("id"));

        var photo = this.Request.Form.Files.FirstOrDefault();

        if (photo == null)
            return BadRequest (new BadFieldResult("photo"));

        try {
            if (res.PhotoUrl != null) storage.TryDelete(res.PhotoUrl);
            string ext = photo.FileName.Split('.').LastOrDefault();
            string url = $"{typeName.ToLower()}-{res.Id}-{Guid.NewGuid()}.{ext}";
            storage.TrySave(photo.OpenReadStream(), url);
            res.PhotoUrl = url;
            dataContext.Resources.Update(res);
            dataContext.SaveChanges();
            return Ok(new SuccessResult("Successfully attached photo to resource"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to attach photo"));
        }
    }

    [HttpPost("set-quantity/{id}")]
    public IActionResult SetQuantity (int id, double quantity)
    {
        var resource = dbQueryableSet.FirstOrDefault(r => r.Id == id);

        if (resource == null) {
            return NotFound ( new BadFieldResult("id") );
        }

        if (quantity > resource.Stored.NormalStock * 3 || quantity < 0) {
            return BadRequest(new BadFieldResult("quantity"));
        } 

        try {
            resource.Stored.InStock = Math.Round(quantity, resource.Precision);
            resource.Stored.LastCheckedAt = DateTime.UtcNow;
            dbSet.Update(resource);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully updated stock quantity"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return BadRequest(new FailResult("Can not update stock quantity"));
        }
    }

}
