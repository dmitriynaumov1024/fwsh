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
using Fwsh.WebApi.Results.Common;
using Fwsh.WebApi.Results.Catalog;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("catalog/fabrics")]
public class FabricController : ControllerBase
{
    const int PAGESIZE = 5;
    const int MAXSIZE = 10;

    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public FabricController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? color = null, int? fabrictype = null, int? page = null)
    {
        IQueryable<Fabric> fabrics = dataContext.Fabrics
            .Include(f => f.Color)
            .Include(f => f.FabricType);

        if (color is int colorid) {
            fabrics = fabrics.Where(f => f.ColorId == colorid);
        }
        if (fabrictype is int fabrictypeid) {
            fabrics = fabrics.Where(f => f.FabricTypeId == fabrictypeid);
        }
        if (page is int pagenumber) {
            return Ok ( fabrics.OrderBy(f => f.Id).Paginate ( 
                pagenumber, PAGESIZE, fabric => new FabricResult(fabric)
            ));
        }
        else {
            return Ok ( fabrics.Listiate (
                MAXSIZE, fabric => new FabricResult(fabric)
            ));
        }
    }

}
