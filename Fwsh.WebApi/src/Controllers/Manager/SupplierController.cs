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
using Fwsh.WebApi.Requests.Manager;
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

        var suppliers = dataContext.Suppliers.Where (s =>
               s.Surname.ToLower().Equals(query)
            || s.Surname.ToLower().Contains(query)
            || s.Name.ToLower().Equals(query) 
            || s.Name.ToLower().Contains(query)
            || s.Name.ToLower().Equals(query)
            || s.OrgName.ToLower().Contains(query)
            || s.Phone.Contains(query)
            || s.Email.Contains(query));

        return Ok ( suppliers.Listiate(MAX_SIZE, supplier => new SupplierResult(supplier)) );
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

    [HttpPost("create")]
    public IActionResult Create (PersonRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }

        Supplier supplier = new Supplier();
        request.ApplyTo(supplier);

        try {
            dataContext.Suppliers.Add(supplier);
            dataContext.SaveChanges();
            return Ok(new CreationResult(supplier.Id, "Successfully created new Supplier"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to create Supplier"));
        }
    }

    [HttpPost("update/{id}")]
    public IActionResult Update (int id, PersonRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }

        Supplier supplier = dataContext.Suppliers
            .FirstOrDefault(s => s.Id == id);
        
        if (supplier == null) {
            return NotFound(new BadFieldResult("id"));
        }

        request.ApplyTo(supplier);

        try {
            dataContext.Suppliers.Update(supplier);
            dataContext.SaveChanges();
            return Ok(new CreationResult(supplier.Id, "Successfully created new Supplier"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong while trying to create Supplier"));
        }
    }
}
