namespace Fwsh.WebApi.Controllers.Catalog;

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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("catalog/materials")]
public class MaterialController : FwshController
{
    const int PAGESIZE = 5;

    public MaterialController (FwshDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1, bool reverse = false) 
    {
        if (page < 0) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<Material> materials = dataContext.Materials
            .Include(m => m.Color)
            .Where(m => m.IsDecorative);
        
        materials = reverse ? 
            materials.OrderByDescending(m => m.Id) :
            materials.OrderBy(m => m.Id);

        return Ok ( materials.Paginate ( 
            page, PAGESIZE, mat => new MaterialResult(mat) 
        ));
    }

}
