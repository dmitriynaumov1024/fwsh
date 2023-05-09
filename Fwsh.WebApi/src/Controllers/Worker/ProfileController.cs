namespace Fwsh.WebApi.Controllers.Worker;

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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Requests.Auth;
using Fwsh.WebApi.Requests.Worker;
using Fwsh.WebApi.Results;

[ApiController]
[Route("worker/profile")]
public class ProfileController : FwshController
{
    public ProfileController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("view")]
    public IActionResult View ()
    {
        int id = user.ConfirmedId;
        var worker = dataContext.Workers
            .FirstOrDefault(w => w.Id == id);
        
        if (worker == null) {
            return NotFound(new MessageResult($"Can not view own profile."));
        }

        return Ok (new WorkerResult(worker)); 
    }

    IActionResult OnUpdate (Worker worker, UpdateRequest<Worker> request)
    {
        try {
            request.ApplyTo(worker);
            dataContext.Workers.Update(worker);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully updated profile"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to update profile"));
        }
    }

    [HttpPost("update")]
    public IActionResult Update (WorkerUpdateRequest request)
    {
        if (request.Validate().State.HasBadFields) 
            return BadRequest (new BadFieldResult(request.State.BadFields));

        int id = user.ConfirmedId;
        var worker = dataContext.Workers.Find(id);

        if (worker == null) 
            return NotFound (new MessageResult($"Can not update own profile"));

        if (request.OldPassword.QuickHash() != worker.Password)
            return BadRequest(new BadFieldResult("oldPassword"));

        return OnUpdate(worker, request);
    }

    [HttpPost("set-password")]
    public IActionResult SetPassword (PasswordUpdateRequest request)
    {
        if (request.Validate().State.HasBadFields)
            return BadRequest (new BadFieldResult(request.State.BadFields));

        int id = user.ConfirmedId;
        var worker = dataContext.Workers.Find(id);

        if (worker == null)
            return NotFound (new MessageResult($"Can not update own profile"));

        if (!request.PasswordMatch(worker))
            return BadRequest(new BadFieldResult("oldPassword"));

        return OnUpdate(worker, request);
    }

    [HttpPost("logout")]
    public IActionResult Logout ()
    {
        user.Destroy();

        return Ok(new MessageResult("Successfully logged out"));
    }

}
