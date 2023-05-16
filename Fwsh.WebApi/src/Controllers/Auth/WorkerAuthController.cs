namespace Fwsh.WebApi.Controllers.Auth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Requests.Auth;
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/worker")]
public class WorkerAuthController : FwshController
{
    public WorkerAuthController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpPost("signup")]
    public IActionResult Signup (WorkerSignupRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new FailResult(request.State.Message ?? "Something went wrong"));
        }

        bool phoneAlreadyExists = dataContext.Workers
            .FirstOrDefault(c => c.Phone == request.Phone) != null; 

        if (phoneAlreadyExists) {
            return Found (new BadFieldResult("phone"));
        }

        try {
            var worker = request.Create();
            dataContext.Workers.Add(worker);
            dataContext.SaveChanges();
            int id = worker.Id;
            return Ok(new CreationResult(id, $"Successfully created {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong"));
        }
    }

    [HttpPost("login")]
    public IActionResult Login (LoginRequest request)
    {
        var worker = dataContext.Workers
            .FirstOrDefault(c => c.Phone == request.Phone);

        if (worker == null) {
            return NotFound(new BadFieldResult("phone"));
        }
        if (worker.Password == request.Password.QuickHash()) {
            user.ConfirmedId = worker.Id;
            user.ConfirmedRole = UserRole.Worker;
            return Ok(new SuccessResult("Successfully logged in"));
        }
        return BadRequest(new BadFieldResult("password"));
    }

    [HttpPost("logout")]
    public IActionResult Logout ()
    {
        user.Destroy();

        return Ok(new MessageResult("Successfully logged out"));
    }

}
