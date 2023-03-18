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
using Fwsh.WebApi.Requests.Resources;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Results.Resources;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("resources/fabrictypes")]
public class FabricTypeController : ControllerBase
{
    const int PAGESIZE = 10;

    protected FwshDataContext dataContext;
    protected Logger logger;
    protected FwshUser user;

    protected virtual bool canCreate => user.ConfirmedRole >= UserRole.Manager;
    protected virtual bool canUpdate => user.ConfirmedRole >= UserRole.Manager;

    public FabricTypeController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1)
    {
        if (page < 0) {
            return BadRequest ( new BadFieldResult("page") );
        }

        var result = dataContext.FabricTypes.OrderBy(r => r.Id)
            .Paginate(page, PAGESIZE, fabricType => new FabricTypeResult(fabricType));

        return Ok (result);
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        var fabricType = dataContext.FabricTypes.Find(id);

        if (fabricType == null) {
            return NotFound (new BadFieldResult("id"));
        }

        return Ok(new FabricTypeResult(fabricType));
    }

    [HttpPost("create")]
    public IActionResult Create (FabricTypeRequest request)
    {
        if (! canCreate) {
            return Unauthorized(new FailResult("Not enough rights to create FabricType"));
        }
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? 
                "Something went wrong while validating fabricType creation request"));
        }

        var fabricType = request.Create();

        try {
            dataContext.FabricTypes.Add(fabricType);
            dataContext.SaveChanges();
            return Ok(new CreationResult(fabricType.Id, $"Successfully created FabricType {fabricType.Id}"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return BadRequest(new FailResult("Failed to create new FabricType"));
        }
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, FabricTypeRequest request)
    {
        if (! canUpdate) {
            return Unauthorized(new FailResult("Not enough rights to update FabricType"));
        }
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? 
                "Something went wrong while validating fabricType creation request"));
        }

        var fabricType = dataContext.FabricTypes.Find(id);
        if (fabricType == null) {
            return NotFound (new BadFieldResult("id"));
        }

        request.ApplyTo(fabricType);

        try {
            dataContext.FabricTypes.Update(fabricType);
            dataContext.SaveChanges();
            return Ok(new SuccessResult($"Successfully updated FabricType {fabricType.Id}"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return BadRequest(new FailResult($"Failed to update FabricType {fabricType.Id}"));
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete (int id)
    {
        if (! canUpdate) {
            return Unauthorized(new FailResult("Not enough rights to delete FabricType"));
        }

        var fabricType = dataContext.FabricTypes.Find(id);
        
        if (fabricType == null) {
            return NotFound (new BadFieldResult("id"));
        }

        int fabricCount = dataContext.Fabrics
            .Where(fabric => fabric.FabricTypeId == id).Count();

        if (fabricCount > 0) {
            return BadRequest(new FailResult($"Can not delete FabricType {id} because of {fabricCount} dependent Fabrics"));
        }

        try {
            dataContext.FabricTypes.Remove(fabricType);
            dataContext.SaveChanges();
            return Ok(new DeletionResult(id, $"Successfully deleted FabricType {id}"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return BadRequest(new FailResult($"Something went wrong while trying to delete FabricType {id}"));
        }
    }

}
