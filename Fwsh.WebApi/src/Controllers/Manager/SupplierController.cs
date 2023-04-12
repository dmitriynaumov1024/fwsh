namespace Fwsh.WebApi.Controllers.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/suppliers")]
public class SupplierController : FwshController
{
    const int PAGESIZE = 10;

    public SupplierController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null)
    {
        if (page == null) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<Supplier> suppliers = dataContext.Suppliers;

        return Ok (suppliers.Paginate((int)page, PAGESIZE, supplier => new SupplierResult(supplier)));
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id) 
    {
        var supplier = dataContext.Suppliers
            .FirstOrDefault(supplier => supplier.Id == id);

        if (supplier == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new SupplierResult(supplier) );
    }

}
