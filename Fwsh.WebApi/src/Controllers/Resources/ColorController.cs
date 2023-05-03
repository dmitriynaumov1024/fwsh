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
using Fwsh.WebApi.Requests.Resources;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("resources/colors")]
public class ColorController : FwshController
{
    const int PAGESIZE = 10;

    protected virtual bool canCreate => user.ConfirmedRole >= UserRole.Manager;
    protected virtual bool canUpdate => user.ConfirmedRole >= UserRole.Manager;

    public ColorController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1, bool reverse = false)
    {
        if (page < 0) {
            return BadRequest ( new BadFieldResult("page") );
        }

        var colors = reverse ? 
            dataContext.Colors.OrderByDescending(r => r.Id) :
            dataContext.Colors.OrderBy(r => r.Id);

        return Ok (
            colors.Paginate(page, PAGESIZE, color => new ColorResult(color))
        );
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        var color = dataContext.Colors.Find(id);

        if (color == null) {
            return NotFound (new BadFieldResult("id"));
        }

        return Ok(new ColorResult(color));
    }

    [HttpPost("create")]
    public IActionResult Create (ColorRequest request)
    {
        if (! canCreate) {
            return Unauthorized(new FailResult("Not enough rights to create Color"));
        }
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? 
                "Something went wrong while validating color creation request"));
        }

        var color = request.Create();

        try {
            dataContext.Colors.Add(color);
            dataContext.SaveChanges();
            return Ok(new CreationResult(color.Id, $"Successfully created Color {color.Id}"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return ServerError (new FailResult("Failed to create new Color"));
        }
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, ColorRequest request)
    {
        if (! canUpdate) {
            return Unauthorized(new FailResult("Not enough rights to update Color"));
        }
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? 
                "Something went wrong while validating color creation request"));
        }

        var color = dataContext.Colors.Find(id);
        if (color == null) {
            return NotFound (new BadFieldResult("id"));
        }

        request.ApplyTo(color);

        try {
            dataContext.Colors.Update(color);
            dataContext.SaveChanges();
            return Ok(new SuccessResult($"Successfully updated Color {color.Id}"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return ServerError (new FailResult($"Failed to update Color {color.Id}"));
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete (int id)
    {
        if (! canUpdate) {
            return Unauthorized(new FailResult("Not enough rights to delete Color"));
        }

        var color = dataContext.Colors.Find(id);
        
        if (color == null) {
            return NotFound (new BadFieldResult("id"));
        }

        int fabricCount = dataContext.Fabrics
            .Where(fabric => fabric.ColorId == id).Count();

        if (fabricCount > 0) {
            return BadRequest(new FailResult($"Can not delete Color {id} because of {fabricCount} dependent Fabrics"));
        }

        int matCount = dataContext.Materials
            .Where(mat => mat.ColorId == id).Count();

        if (matCount > 0) {
            return BadRequest(new FailResult($"Can not delete Color {id} because of {matCount} dependent Materials"));
        }

        try {
            dataContext.Colors.Remove(color);
            dataContext.SaveChanges();
            return Ok(new DeletionResult(id, $"Successfully deleted Color {id}"));
        }
        catch (Exception ex) {
            logger.Error("{0}", ex);
            return ServerError(new FailResult($"Something went wrong while trying to delete Color {id}"));
        }
    }

}
