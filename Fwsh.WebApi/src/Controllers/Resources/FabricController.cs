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
[Route("resources/fabrics")]
public class FabricController : ResourceController<double, Fabric, StoredFabric, StoredFabricResult>
{
    protected override DbSet<StoredFabric> dbSet => 
        dataContext.StoredFabrics;

    protected override IQueryable<StoredFabric> dbQueryableSet => 
        dataContext.StoredFabrics
            .Include(r => r.Supplier)
            .Include(r => r.Item.FabricType)
            .Include(f => f.Item.Color);

    public FabricController (FwshDataContext dataContext, Logger logger, FwshUser user)
    : base (dataContext, logger, user) 
    { 

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
}
