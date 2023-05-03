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

    public FabricTypeController (FwshDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1, bool reverse = false) 
    {
        if (page < 0) {
            return BadRequest(new BadFieldResult("page"));
        }

        var fabricTypes = reverse ?
            dataContext.FabricTypes.OrderByDescending(f => f.Id) :
            dataContext.FabricTypes.OrderBy(f => f.Id);

        return Ok ( fabricTypes.Paginate (
            page, PAGESIZE, ftype => new FabricTypeResult(ftype)
        ));
    }

}
