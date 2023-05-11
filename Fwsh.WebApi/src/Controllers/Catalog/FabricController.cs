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
[Route("catalog/fabrics")]
public class FabricController : FwshController
{
    const int PAGESIZE = 5;
    const int MAXSIZE = 10;

    public FabricController (FwshDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [HttpGet("list")]
    public IActionResult List (int? color = null, int? fabrictype = null, int? page = null, bool reverse = false)
    {
        IQueryable<Resource> fabrics = dataContext.Resources
            .Include(f => f.Color)
            .Include(f => f.FabricType)
            .Where(f => f.SlotName == SlotNames.Fabric);

        if (color is int colorid) {
            fabrics = fabrics.Where(f => f.ColorId == colorid);
        }
        if (fabrictype is int fabrictypeid) {
            fabrics = fabrics.Where(f => f.FabricTypeId == fabrictypeid);
        }
        if (page is int pagenumber) {
            fabrics = reverse ? 
                fabrics.OrderByDescending(f => f.Id) :
                fabrics.OrderBy(f => f.Id);
            return Ok ( fabrics.Paginate ( 
                pagenumber, PAGESIZE, fabric => new ResourceResult(fabric)
            ));
        }
        else {
            return Ok ( fabrics.Listiate (
                MAXSIZE, fabric => new ResourceResult(fabric)
            ));
        }
    }

}
