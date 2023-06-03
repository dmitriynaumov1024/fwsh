namespace Fwsh.WebApi.Controllers.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        IQueryable<Design> designs = dataContext.Designs;

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
            .FirstOrDefault(d => d.Id == id);

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
            dataContext.Designs.Update(design);
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
            .Include(d => d.Tasks)
            .ThenInclude(t => t.Resources)
            .ThenInclude(p => p.Item);
        
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
            .FirstOrDefault(d => d.Id == designId);

        if (design == null) {
            return BadRequest (new BadFieldResult("id"));
        }

        var requestPhotos = this.Request.Form.Files.ToList();
        var photoUrls = design.PhotoUrls.ToList();

        int count = 0, pos = design.PhotoUrls.Count + 1;

        foreach (var photo in requestPhotos) {
            if (photoUrls.Count >= MAX_PHOTOS) break;
            string ext = photo.FileName.Split('.').LastOrDefault();
            string url = $"design-{design.Id}-{pos}-{Guid.NewGuid()}.{ext}"; 
            if (storage.TrySave(photo.OpenReadStream(), url)) {
                photoUrls.Add(url);
                count += 1;
                pos += 1;
            }
        }

        try {
            design.PhotoUrls = photoUrls;
            dataContext.Designs.Update(design);
            dataContext.SaveChanges();
            return Ok(new SuccessResult($"Successfully attached {count} photos to Design {designId}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to attach photos"));
        }
    }

    [HttpPost("delete-photos/{designId}")]
    public IActionResult DeletePhotos (int designId, List<string> urls)
    {
        Design design = dataContext.Designs
            .FirstOrDefault(d => d.Id == designId);

        if (design == null) 
            return NotFound(new BadFieldResult("id"));

        try {
            int count = 0;
            var photoUrls = design.PhotoUrls?.ToList() ?? new List<string>();

            foreach (var photoUrl in photoUrls.ToList()) {
                if (urls.Contains(photoUrl)) {
                    storage.TryDelete(photoUrl);
                    photoUrls.Remove(photoUrl);
                    count += 1;
                }
            }

            design.PhotoUrls = photoUrls;
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
            .Include(design => design.Tasks)
            .ThenInclude(task => task.Resources)
            .FirstOrDefault(design => design.Id == id);

        if (design == null) {
            return NotFound(new BadFieldResult("id"));
        }

        bool canDelete = dataContext.ProdOrders
            .Where(order => order.IsActive == true)
            .Where(order => order.DesignId == design.Id)
            .Count() == 0;

        if (! canDelete) {
            return BadRequest(new FailResult($"Can not delete Design {id} because of dependent production orders"));
        }

        foreach (var photoUrl in design.PhotoUrls) {
            storage.TryDelete(photoUrl);
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
