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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Results.Catalog;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("catalog/materials")]
public class MaterialController : ControllerBase
{
    const int PAGESIZE = 5;

    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public MaterialController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1) 
    {
        if (page < 0) {
            return BadRequest(new BadFieldResult("page"));
        }

        var materials = dataContext.Materials
            .Include(m => m.Color)
            .Where(m => m.IsDecorative)
            .OrderBy(m => m.Id);

        return Ok ( materials.Paginate(page, PAGESIZE, m => new DecorMaterialResult(m)) );
    }

}
