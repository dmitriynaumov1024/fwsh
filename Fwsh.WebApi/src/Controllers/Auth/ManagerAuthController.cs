namespace Fwsh.WebApi.Controllers;

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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;

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
        if (!env.isDevelopment) {
            return NotFound (new {
                Message = "Not found"
            });
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
            Password = request.Password.SHA512Hash(),
            Roles = new HashSet<WorkerRole> { 
                new WorkerRole { RoleName = Roles.Management } 
            }
        };

        try {
            dataContext.Workers.Add(storedWorker);
            dataContext.SaveChanges();
            return Ok(new MessageResult($"Successfully created {storedWorker.Id}"));
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
            && storedWorker.Roles.FirstOrDefault(role => role.RoleName == Roles.Management) != null;

        if (!isManager) {
            return NotFound(new BadFieldResult("phone"));
        }

        if (storedWorker.Password == request.Password.SHA512Hash()) {
            user.ConfirmedId = storedWorker.Id;
            user.ConfirmedRole = UserRole.Manager;
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
