namespace Fwsh.WebApi.Controllers.Manager;

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
using Fwsh.WebApi.Requests.Worker;
using Fwsh.WebApi.Results;

[ApiController]
[Route("manager/profile")]
public class ProfileController : FwshController
{
    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

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
        var manager = dataContext.Workers
            .Include(w => w.Roles)
            .FirstOrDefault(w => w.Id == id);
        
        if (manager == null) {
            return NotFound (new MessageResult($"Can not view own profile."));
        }

        return Ok (new WorkerResult(manager)); 
    }

    [HttpPost("update")]
    public IActionResult Update (WorkerUpdateRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest (new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest (new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        int id = user.ConfirmedId;
        var storedWorker = dataContext.Workers
            .Include(w => w.Roles)
            .FirstOrDefault(w => w.Id == id);

        if (storedWorker == null) {
            return NotFound (new MessageResult($"Can not update own profile"));
        }
        if (storedWorker.Password != request.OldPassword.QuickHash()) {
            return BadRequest (new BadFieldResult("oldPassword"));
        }

        try {
            storedWorker.Password = request.NewPassword.QuickHash();
            dataContext.Workers.Update(storedWorker);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Profile updated successfully"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong"));
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout ()
    {
        user.Destroy();

        return Ok (new MessageResult("Successfully logged out"));
    }

}
