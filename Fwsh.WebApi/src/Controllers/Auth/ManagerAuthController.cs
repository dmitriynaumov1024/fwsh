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
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Requests.Auth;
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/manager")]
public class ManagerAuthController : FwshController
{
    public ManagerAuthController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpPost("signup")]
    public IActionResult Signup (ManagerSignupRequest request)
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
            .FirstOrDefault(c => c.Phone == request.Phone) != null; 

        if (phoneAlreadyExists) {
            return BadRequest(new BadFieldResult("phone"));
        }

        try {
            var manager = request.Create();
            dataContext.Workers.Add(manager);
            dataContext.SaveChanges();
            int id = manager.Id;
            return Ok(new CreationResult(id, $"Successfully created {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new MessageResult("Something went wrong"));
        }
    }

    [HttpPost("login")]
    public IActionResult Login (LoginRequest request)
    {
        var manager = dataContext.Workers
            .FirstOrDefault(c => c.Phone == request.Phone);
        
        bool isManager = manager != null && manager.IsManager;

        if (!isManager) {
            return NotFound(new BadFieldResult("phone"));
        }

        if (manager.Password == request.Password.QuickHash()) {
            user.ConfirmedId = manager.Id;
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
