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
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Requests.Auth;
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/worker")]
public class WorkerAuthController : ControllerBase
{
    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public WorkerAuthController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpPost("signup")]
    public IActionResult Signup (WorkerSignupRequest request)
    {
        bool phoneAlreadyExists = dataContext.Workers
            .Where(c => c.Phone == request.Phone)
            .FirstOrDefault() != null; 

        if (phoneAlreadyExists) {
            return BadRequest(new BadFieldResult("phone"));
        }

        var storedWorker = new Worker() {
            Surname = request.Surname,
            Name = request.Name,
            Patronym = request.Patronym,
            Phone = request.Phone,
            Email = request.Email,
            Password = request.Password.QuickHash(),
            Roles = new HashSet<WorkerRole> ( 
                new HashSet<string>(request.Roles)
                    .Where(role => Roles.KnownWorkerRoles.Contains(role))
                    .Select(role => new WorkerRole() { RoleName = role })
            )
        };

        try {
            dataContext.Workers.Add(storedWorker);
            dataContext.SaveChanges();
            int id = storedWorker.Id;
            return Ok(new CreationResult(id, $"Successfully created {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
        }
        return BadRequest(new MessageResult("Something went wrong"));
    }

    [HttpPost("login")]
    public IActionResult Login (LoginRequest request)
    {
        var storedWorker = dataContext.Workers
            .Where(c => c.Phone == request.Phone)
            .FirstOrDefault();

        if (storedWorker == null) {
            return NotFound(new BadFieldResult("phone"));
        }
        if (storedWorker.Password == request.Password.QuickHash()) {
            user.ConfirmedId = storedWorker.Id;
            user.ConfirmedRole = UserRole.Worker;
            return Ok();
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
