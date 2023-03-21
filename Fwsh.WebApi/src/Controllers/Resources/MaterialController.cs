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
[Route("resources/materials")]
public class MaterialController : ResourceController<double, Material, StoredMaterial, StoredMaterialResult>
{
    protected override DbSet<StoredMaterial> dbSet => 
        dataContext.StoredMaterials;

    protected override IQueryable<StoredMaterial> dbQueryableSet => 
        dataContext.StoredMaterials
            .Include(r => r.Supplier)
            .Include(r => r.Item.Color);

    public MaterialController (FwshDataContext dataContext, Logger logger, FwshUser user)
    : base (dataContext, logger, user) 
    { 

    }

    protected override IResultBuilder<StoredMaterialResult> ResultBuilder (StoredMaterial mat)
    {
        return new StoredMaterialResult(mat);
    }

    [HttpPost("create")]
    public IActionResult Create (MaterialCreationRequest request)
    {
        return base.OnCreate(request);
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, MaterialUpdateRequest request)
    {
        return base.OnUpdate(id, request);
    }
}
