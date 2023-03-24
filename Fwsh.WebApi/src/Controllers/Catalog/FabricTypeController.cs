namespace Fwsh.WebApi.Controllers.Catalog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
[Route("catalog/fabrictypes")]
public class FabricTypeController : FwshController
{
    const int PAGESIZE = 5;

    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public FabricTypeController (FwshDataContext dataContext, Logger logger, FwshUser user)
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

        var fabricTypes = dataContext.FabricTypes
            .OrderBy(ftype => ftype.Id)
            .Paginate(page, PAGESIZE, ftype => new FabricTypeResult(ftype));

        return Ok(fabricTypes);
    }

}
