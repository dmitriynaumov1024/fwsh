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
using Fwsh.WebApi.Requests.Manager;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/taskprototypes")]
public class TaskPrototypeController : FwshController
{
    const int MAX_SIZE = 20;

    public TaskPrototypeController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("list")]
    public IActionResult List (int? design = null)
    {
        if (design == null) 
            return NotFound(new BadFieldResult("design"));

        var existingDesign = dataContext.Designs
            .Include(d => d.Tasks)
            .FirstOrDefault(d => d.Id == design);

        if (existingDesign == null) 
            return NotFound(new BadFieldResult("design"));

        return Ok ( new ListResult<TaskPrototypeResult> (
            existingDesign.Tasks
                .OrderBy(t => t.Precedence).ThenBy(t => t.Id)
                .Select(t => new TaskPrototypeResult(t).Mini())
        ));
    }

    [HttpGet("view/{id}")]
    public IActionResult View (int id)
    {
        var task = dataContext.TaskPrototypes
            .Include(t => t.Design)
            .Include(t => t.Resources)
            .ThenInclude(r => r.Item.Stored)
            .FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound(new BadFieldResult("id"));

        return Ok (new TaskPrototypeResult(task).ForManager());
    }

    [HttpPost("create")]
    public IActionResult Create (int design, TaskPrototypeRequest request)
    {
        if (request.Validate().State.HasBadFields || !request.State.IsValid) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }

        if (dataContext.Designs.FirstOrDefault(d => d.Id == design) == null) {
            return BadRequest(new BadFieldResult("design"));
        }

        TaskPrototype task = request.Create();
        task.DesignId = design;

        try {
            dataContext.TaskPrototypes.Add(task);
            dataContext.SaveChanges();
            return Ok(new CreationResult(task.Id, "Successfully created Task Prototype"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not create Task Prototype"));
        }
    }

    [HttpPost("update")]
    public IActionResult Update (int id, TaskPrototypeRequest request)
    {
        if (request.Validate().State.HasBadFields || !request.State.IsValid) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }

        var task = dataContext.TaskPrototypes
            .Include(t => t.Resources)
            .ThenInclude(r => r.Item.Stored)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) {
            return NotFound(new BadFieldResult("id"));
        }
        
        try {
            request.ApplyTo(task);
            dataContext.TaskPrototypes.Update(task);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully updated Task Prototype"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Can not update Task Prototype"));
        }
    }
}
