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
public class FabricController : ResourceController
{
    protected override string typeName => ResourceTypes.Fabric;

    protected override IQueryable<Resource> dbQueryableSet => 
        dataContext.Resources
            .Include(r => r.Stored)
            .Include(r => r.Stored.Supplier)
            .Include(r => r.FabricType)
            .Include(r => r.Color)
            .Where(r => r.Type == ResourceTypes.Fabric);

    public FabricController (FwshDataContext dataContext, Logger logger, FwshUser user, FileStorageProvider storage)
    : base (dataContext, logger, user) 
    { 
        this.storage = storage;
    }

    [HttpPost("create")]
    public IActionResult Create (FabricRequest request)
    {
        return base.OnCreate(request);
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, FabricRequest request)
    {
        return base.OnUpdate(id, request);
    }
}
