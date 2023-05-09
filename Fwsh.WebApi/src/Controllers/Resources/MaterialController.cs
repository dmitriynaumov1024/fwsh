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
[Route("resources/materials")]
public class MaterialController : ResourceController
{
    protected override string typeName => ResourceTypes.Material;

    protected override IQueryable<StoredResource> dbQueryableSet => 
        dataContext.StoredResources
            .Include(r => r.Supplier)
            .Include(r => r.Color)
            .Where(r => r.Type == ResourceTypes.Material);

    public MaterialController (FwshDataContext dataContext, Logger logger, FwshUser user, FileStorageProvider storage)
    : base (dataContext, logger, user) 
    { 
        this.storage = storage;
    }

    [HttpPost("create")]
    public IActionResult Create (StoredMaterialRequest request)
    {
        return base.OnCreate(request);
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, StoredMaterialRequest request)
    {
        return base.OnUpdate(id, request);
    }
}
