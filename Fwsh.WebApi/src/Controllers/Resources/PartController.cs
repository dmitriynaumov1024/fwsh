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
public class PartController : ResourceController
{
    protected override string typeName => ResourceTypes.Part;

    protected override IQueryable<StoredResource> dbQueryableSet => 
        dataContext.StoredResources
            .Include(r => r.Supplier)
            .Where(r => r.Type == ResourceTypes.Part);

    public PartController (FwshDataContext dataContext, Logger logger, FwshUser user)
    : base (dataContext, logger, user) 
    { 

    }

    [HttpPost("create")]
    public IActionResult Create (StoredPartRequest request)
    {
        return base.OnCreate(request);
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, StoredPartRequest request)
    {
        return base.OnUpdate(id, request);
    }
}
