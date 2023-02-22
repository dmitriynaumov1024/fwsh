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
[Route("worker/profile")]
public class WorkerProfileController : ControllerBase
{
    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public WorkerProfileController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("view")]
    public IActionResult View ()
    {
        int id = user.ConfirmedId;
        var storedWorker = dataContext.Workers
            .Include(w => w.Roles)
            .FirstOrDefault(w => w.Id == id);
        
        if (storedWorker == null) {
            return NotFound(new MessageResult($"Can not view own profile."));
        }

        return Ok (new WorkerResult(storedWorker)); 
    }

    [HttpPost("update")]
    public IActionResult Update (WorkerUpdateRequest request)
    {
        int id = user.ConfirmedId;
        var storedWorker = dataContext.Workers
            .Include(w => w.Roles)
            .FirstOrDefault(w => w.Id == id);

        if (storedWorker == null) {
            return NotFound(new MessageResult($"Can not update own profile"));
        }
        if (storedWorker.Password != request.OldPassword.SHA512Hash()) {
            return BadRequest(new BadFieldResult("oldPassword"));
        }

        storedWorker.Password = request.NewPassword.SHA512Hash();
        dataContext.Workers.Update(storedWorker);
        dataContext.SaveChanges();
        return Ok(new MessageResult("Profile updated successfully"));
    }

    [HttpPost("logout")]
    public IActionResult Logout ()
    {
        user.Destroy();

        return Ok(new MessageResult("Successfully logged out"));
    }

}
