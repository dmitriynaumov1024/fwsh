namespace Fwsh.WebApi.Controllers.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.FileStorage;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Requests.Manager;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/designs")]
public class DesignController : FwshController
{
    const int PAGESIZE = 10;
    const int MAX_PHOTOS = 10;

    protected FileStorageProvider storage;

    public DesignController (FwshDataContext dataContext, Logger logger, FwshUser user, FileStorageProvider storage)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
        this.storage = storage;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, string type = null) 
    {
        IQueryable<Design> designs = dataContext.Designs
            .Include(d => d.Photos);

        if (type != null) {
            designs = designs.Where(d => d.Type == type);
        }
        if (page is int pagenumber) {
            return Ok ( designs.OrderBy(d => d.Id).Paginate (
                pagenumber, PAGESIZE, design => new DesignResult(design).Mini()
            ));
        }
        else {
            return BadRequest(new BadFieldResult("page"));
        }
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        Design design = dataContext.Designs
            .Include(d => d.Photos)
            .Where(d => d.Id == id)
            .FirstOrDefault();

        if (design == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok (new DesignResult(design).ForManager());
    }

    [HttpPost("create")]
    public IActionResult Create (DesignRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest (new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest (new FailResult(request.State.Message ?? "Something went wrong"));
        }

        if (dataContext.Designs.FirstOrDefault(d => d.NameKey == request.NameKey) != null) {
            return BadRequest (new BadFieldResult("nameKey"));
        }

        try {
            Design design = request.Create();
            dataContext.Designs.Add(design);
            dataContext.SaveChanges();
            return Ok (new CreationResult(design.Id, $"Successfully created Design {design.Id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to create new Design"));
        }
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, DesignRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest (new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest (new FailResult(request.State.Message ?? "Something went wrong"));
        }

        Design design = dataContext.Designs.FirstOrDefault(d => d.Id == id);
        
        if (design == null) {
            return NotFound (new BadFieldResult("id"));
        }

        try {
            request.ApplyTo(design);
            dataContext.Designs.Add(design);
            dataContext.SaveChanges();
            return Ok (new CreationResult(design.Id, $"Successfully created Design {design.Id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to create new Design"));
        }
    }

    [HttpPost("recalculate")]
    public IActionResult Recalculate (int? id = null) 
    {
        IQueryable<Design> designs = dataContext.Designs
            .Include(d => d.TaskPrototypes).ThenInclude(t => t.Parts).ThenInclude(p => p.Item)
            .Include(d => d.TaskPrototypes).ThenInclude(t => t.Materials).ThenInclude(m => m.Item)
            .Include(d => d.TaskPrototypes).ThenInclude(t => t.Fabrics).ThenInclude(f => f.Item);
        
        if (id != null) 
            designs = designs.Where(design => design.Id == id);
        
        int count = 0; 
        foreach (var design in designs.ToList()) {
            design.UpdatePrice();
            design.UpdateResourceQuantities();
            dataContext.Designs.Update(design);
            count += 1;
        }

        try {
            dataContext.SaveChanges();
            logger.Log("Successfully recalculated {0} designs", count);
            if (id != null) { 
                return Ok (new SuccessResult($"Successfully recalculated Design {id}"));
            }
            else { 
                return Ok (new SuccessResult("Successfully recalculated all designs"));
            } 
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError("Something went wrong while trying to recalculate designs");
        }
    }

    [HttpPost("set-visible/{id}")]
    public IActionResult SetVisible (int id, string visible)
    {
        Design design = dataContext.Designs
            .FirstOrDefault(d => d.Id == id);

        design.IsVisible = visible?.ToLower() switch { "true" => true, _ => false };

        try {
            dataContext.Designs.Update(design);
            dataContext.SaveChanges();
            return Ok (new SuccessResult($"Successfully set visible={design.IsVisible} for Design {id}"));
        } 
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to set visibility of Design"));
        }
    }

    [HttpPost("attach-photos/{designId}")]
    public IActionResult AttachPhotos (int designId) 
    {
        Design design = dataContext.Designs
            .Include(d => d.Photos)
            .FirstOrDefault(d => d.Id == designId);

        var requestPhotos = this.Request.Form.Files.ToList();

        int count = 0, 
            pos = design.Photos.Count > 0 ? design.Photos.Max(p => p.Position) + 1 : 1;

        foreach (var photo in requestPhotos) {
            if (design.Photos.Count >= MAX_PHOTOS) break;
            string ext = photo.FileName.Split('.').LastOrDefault();
            string url = $"design-{design.Id}-{pos}-{Guid.NewGuid()}.{ext}"; 
            if (storage.TrySave(photo.OpenReadStream(), url)) {
                design.Photos.Add(new DesignPhoto { 
                    Url = url, 
                    Position = pos 
                });
                count += 1;
                pos += 1;
            }
        }

        try {
            dataContext.Designs.Update(design);
            dataContext.SaveChanges();
            return Ok(new SuccessResult($"Successfully attached {count} photos to Design {designId}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError("Something went wrong while trying to attach photos");
        }
    }

    [HttpPost("delete-photos/{designId}")]
    public IActionResult DeletePhotos (int designId, [FromBody] IList<string> urls)
    {
        Design design = dataContext.Designs
            .Include(d => d.Photos)
            .FirstOrDefault(d => d.Id == designId);

        int count = 0;
        foreach (var photo in design.Photos.ToList()) {
            if (urls.Contains(photo.Url)) {
                storage.TryDelete(photo.Url);
                design.Photos.Remove(photo);
                count += 1;
            }
        }

        try {
            dataContext.Designs.Update(design);
            dataContext.SaveChanges();
            return Ok(new SuccessResult($"Successfully deleted {count} photos of Design {designId}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to delete photos"));
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete (int id) 
    {
        Design design = dataContext.Designs
            .Include(design => design.Photos)
            .Include(design => design.TaskPrototypes)
                .ThenInclude(task => task.Parts)
            .Include(design => design.TaskPrototypes)
                .ThenInclude(task => task.Fabrics)
            .Include(design => design.TaskPrototypes)
                .ThenInclude(task => task.Materials)
            .FirstOrDefault(design => design.Id == id);

        if (design == null) {
            return NotFound(new BadFieldResult("id"));
        }

        bool canDelete = dataContext.ProductionOrders
            .Where(order => order.DesignId == design.Id)
            .Count() == 0;

        if (! canDelete) {
            return BadRequest(new FailResult($"Can not delete Design {id} because of dependent production orders"));
        }

        foreach (var photo in design.Photos) {
            if (photo.Url != null) storage.TryDelete(photo.Url);
        }

        foreach (var task in design.TaskPrototypes) {
            if (task.InstructionUrl != null) storage.TryDelete(task.InstructionUrl);
        }

        try {
            dataContext.Designs.Remove(design);
            dataContext.SaveChanges();
            return Ok(new DeletionResult(id, $"Successfully deleted Design {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong"));
        }
    }
}
