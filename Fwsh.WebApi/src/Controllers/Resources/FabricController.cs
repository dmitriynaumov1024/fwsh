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
using Fwsh.WebApi.Requests.Resources;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.FileStorage;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("resources/fabrics")]
public class FabricController : ResourceController<double, Fabric, StoredFabric, StoredFabricResult>
{
    private FileStorageProvider storage;

    protected override DbSet<StoredFabric> dbSet => 
        dataContext.StoredFabrics;

    protected override IQueryable<StoredFabric> dbQueryableSet => 
        dataContext.StoredFabrics
            .Include(r => r.Supplier)
            .Include(r => r.Item.FabricType)
            .Include(f => f.Item.Color);

    public FabricController (FwshDataContext dataContext, Logger logger, FwshUser user, FileStorageProvider storage)
    : base (dataContext, logger, user) 
    { 
        this.storage = storage;
    }

    protected override IResultBuilder<StoredFabricResult> ResultBuilder (StoredFabric fabric)
    {
        return new StoredFabricResult(fabric);
    }

    [HttpPost("create")]
    public IActionResult Create (FabricCreationRequest request)
    {
        return base.OnCreate(request);
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, FabricUpdateRequest request)
    {
        return base.OnUpdate(id, request);
    }

    [HttpPost("attach-photo/{id}")]
    public IActionResult AttachPhoto (int id)
    {
        Fabric fabric = dataContext.Fabrics.Find(id);
        
        if (fabric == null) 
            return BadRequest (new BadFieldResult("id"));

        var photo = this.Request.Form.Files.FirstOrDefault();

        if (photo == null)
            return BadRequest (new BadFieldResult("photo"));

        try {
            storage.TryDelete(fabric.PhotoUrl);
            string ext = photo.FileName.Split('.').LastOrDefault();
            string url = $"fabric-{fabric.Id}-{Guid.NewGuid()}.{ext}";
            storage.TrySave(photo.OpenReadStream(), url);
            fabric.PhotoUrl = url;
            dataContext.SaveChanges();
            return Ok(new SuccessResult("Successfully attached photo to Fabric"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to attach photo"));
        }
    }

    [HttpPost("set-quantity/{id}")]
    public IActionResult SetQuantity (int id, [FromBody] double quantity)
    {
        var storedResource = dbSet.Find(id);

        if (storedResource == null) {
            return NotFound ( new BadFieldResult("id") );
        }

        if (quantity > storedResource.NormalStock * 3 || quantity < 0) {
            return BadRequest(new BadFieldResult("quantity"));
        } 

        return base.OnSetQuantity(id, storedResource, quantity);
    }
}
