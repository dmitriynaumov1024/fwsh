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
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("catalog/designs")]
public class DesignController : FwshController
{
    const int PAGESIZE = 5;
    const int MAXSIZE = 10;

    public DesignController (FwshDataContext dataContext)
    {
        this.dataContext = dataContext;
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
                pagenumber, PAGESIZE, design => new DesignResult(design).Mini()
            ));
        }
        else {
            return Ok ( designs.Listiate (
                MAXSIZE, design => new DesignResult(design).Mini()
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

        return Ok (new DesignResult(design).ForCustomer());

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

        return Ok (new DesignResult(design).ForCustomer());
    }

}
