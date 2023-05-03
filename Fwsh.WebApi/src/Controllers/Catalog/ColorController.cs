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
[Route("catalog/colors")]
public class ColorController : FwshController
{
    const int PAGESIZE = 10;

    public ColorController (FwshDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [HttpGet("list")]
    public IActionResult List (int page = -1, bool reverse = false) 
    {
        if (page < 0) {
            return BadRequest(new BadFieldResult("page"));
        }

        var colors = reverse ? 
            dataContext.Colors.OrderByDescending(r => r.Id) :
            dataContext.Colors.OrderBy(r => r.Id);

        return Ok (
            colors.Paginate(page, PAGESIZE, color => new ColorResult(color))
        );
    }

}
