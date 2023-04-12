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
[Route("manager/workers")]
public class WorkerController : FwshController
{
    const int PAGESIZE = 10;

    public WorkerController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? page = null, string role = null)
    {
        if (page == null) {
            return BadRequest(new BadFieldResult("page"));
        }

        IQueryable<Worker> workers = dataContext.Workers
            .Include(worker => worker.Roles);

        if (role != null) {
            workers = workers.Where(worker => worker.Roles.Any(r => r.RoleName == role));
        }

        return Ok (workers.Paginate((int)page, PAGESIZE, worker => new WorkerResult(worker)));
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id) 
    {
        var worker = dataContext.Workers
            .Include(worker => worker.Roles)
            .FirstOrDefault(worker => worker.Id == id);

        if (worker == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new WorkerResult(worker) );
    }

}
