namespace Fwsh.WebApi.Controllers.Catalog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
[Route("catalog/designs")]
public class DesignController : ControllerBase
{
    const int PAGESIZE = 5;
    const int MAXSIZE = 10;

    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public DesignController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, string type = null) 
    {
        IQueryable<Design> designs = dataContext.Designs
            .Include(d => d.Photos);

        if (type != null) {
            designs = designs.Where(d => d.Type == type);
        }
        if (page is int pagenumber) {
            return Ok ( designs.OrderBy(d => d.Id).Paginate (
                pagenumber, PAGESIZE, design => new MiniDesignResult(design)
            ));
        }
        else {
            return Ok ( designs.Listiate (
                MAXSIZE, design => new MiniDesignResult(design)
            ));
        }
    }

    [HttpGet("view/{id:int}")]
    public IActionResult View (int id)
    {
        Design design = dataContext.Designs
            .Include(d => d.Photos)
            .Where(d => d.Id == id)
            .FirstOrDefault();

        if (design == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok (new DesignResult(design));

    }

    [HttpGet("view/{namekey}")]
    public IActionResult View (string namekey)
    {
        Design design = dataContext.Designs
            .Include(d => d.Photos)
            .Where(d => d.NameKey == namekey)
            .FirstOrDefault();

        if (design == null) {
            return NotFound(new BadFieldResult("namekey"));
        }

        return Ok (new DesignResult(design));
    }

}
