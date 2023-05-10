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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/workers")]
public class WorkerController : FwshController
{
    const int PAGESIZE = 10;
    const int MAX_SIZE = 100;

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

        IQueryable<Worker> workers = dataContext.Workers;

        if (role != null) {
            workers = workers.Where(E.Worker.HasRole(role));
        }

        return Ok (workers.Paginate((int)page, PAGESIZE, worker => new WorkerResult(worker)));
    }

    [HttpGet("search")]
    public IActionResult Search (string query)
    {
        if (query == null || query.Length < 2 || query.Trim().Length < 2)
            return BadRequest(new BadFieldResult("query"));

        query = query.Trim().ToLower();

        IEnumerable<Worker> workers = dataContext.Workers.ToList();
        
        workers = workers.Where(w => 
               w.Surname.ToLower().Equals(query)
            || w.Surname.ToLower().Contains(query) 
            || w.Name.ToLower().Equals(query)
            || w.Name.ToLower().Contains(query)
            || w.Phone.Contains(query)
            || w.Email.Contains(query));

        return Ok (new ListResult<WorkerResult>() {
            Items = workers.Select(worker => new WorkerResult(worker)).ToList()
        });
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id) 
    {
        var worker = dataContext.Workers
            .FirstOrDefault(worker => worker.Id == id);

        if (worker == null) {
            return NotFound(new BadFieldResult("id"));
        }

        return Ok ( new WorkerResult(worker) );
    }

}
