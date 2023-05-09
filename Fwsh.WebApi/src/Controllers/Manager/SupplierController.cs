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
    const int MAX_SIZE = 100;

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

    [HttpGet("search")]
    public IActionResult Search (string query)
    {
        if (query == null || query.Length < 2 || query.Trim().Length < 2)
            return BadRequest(new BadFieldResult("query"));

        query = query.Trim().ToLower();

        IEnumerable<Supplier> suppliers = dataContext.Suppliers.ToList();
        suppliers = suppliers.Where (s =>
            s.Surname.ToLower().Equals(query)
            || s.Surname.ToLower().Contains(query)
            || s.Name.ToLower().Equals(query) 
            || s.Name.ToLower().Contains(query)
            || s.Name.ToLower().Equals(query)
            || s.OrgName.ToLower().Contains(query)
            || s.Phone.Contains(query)
            || s.Email.Contains(query));

        return Ok (new ListResult<SupplierResult>() { 
            Items = suppliers.Select(supplier => new SupplierResult(supplier)).ToList()
        });
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
