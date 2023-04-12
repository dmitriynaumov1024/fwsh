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
[Route("manager/customers")]
public class CustomerController : FwshController
{
    const int PAGESIZE = 10;

    public CustomerController (FwshDataContext dataContext, Logger logger, FwshUser user)
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

        IQueryable<Customer> customers = dataContext.Customers;

        return Ok (customers.Paginate((int)page, PAGESIZE, customer => new CustomerResult(customer)));
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id) 
    {
        var customer = dataContext.Customers
            .FirstOrDefault(customer => customer.Id == id);

        if (customer == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new CustomerResult(customer) );
    }

}
