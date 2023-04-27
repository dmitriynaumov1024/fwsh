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
using Fwsh.WebApi.Utils;

[ApiController]
[Route("resources/parts")]
public class PartController : ResourceController<int, Part, StoredPart, StoredPartResult>
{
    protected override DbSet<StoredPart> dbSet => 
        dataContext.StoredParts;

    protected override IQueryable<StoredPart> dbQueryableSet => 
        dataContext.StoredParts
            .Include(r => r.Supplier)
            .Include(r => r.Item);

    public PartController (FwshDataContext dataContext, Logger logger, FwshUser user)
    : base (dataContext, logger, user) 
    { 

    }

    protected override IResultBuilder<StoredPartResult> ResultBuilder (StoredPart part)
    {
        return new StoredPartResult(part);
    }

    [HttpPost("create")]
    public IActionResult Create (PartCreationRequest request)
    {
        return base.OnCreate(request);
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, PartUpdateRequest request)
    {
        return base.OnUpdate(id, request);
    }

    [HttpPost("set-quantity/{id}")]
    public IActionResult SetQuantity (int id, [FromBody] int quantity)
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
