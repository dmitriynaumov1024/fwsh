namespace Fwsh.WebApi.Controllers.Auth;

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
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Requests.Auth;
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/manager")]
public class ManagerAuthController : ControllerBase
{
    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public ManagerAuthController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpPost("signup")]
    public IActionResult Signup (WorkerSignupRequest request)
    {
        // forbid manager signup in prod. environment
        if (!env.isDevelopment) {
            return NotFound (new MessageResult("Not found"));
        }

        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new MessageResult(request.State.Message ?? "Something went wrong"));
        }

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
            Roles = new HashSet<WorkerRole> { 
                new WorkerRole { RoleName = Roles.Management } 
            }
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
            .Include(w => w.Roles)
            .Where(c => c.Phone == request.Phone)
            .FirstOrDefault();
        
        bool isManager = storedWorker != null 
            && storedWorker.Roles.Any(role => role.RoleName == Roles.Management);

        if (!isManager) {
            return NotFound(new BadFieldResult("phone"));
        }

        if (storedWorker.Password == request.Password.QuickHash()) {
            user.ConfirmedId = storedWorker.Id;
            user.ConfirmedRole = UserRole.Manager;
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
