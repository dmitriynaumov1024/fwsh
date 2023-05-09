namespace Fwsh.WebApi.Controllers.Auth;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Fwsh.Utils;
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.SillyAuth; 
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/fake-auth")]
public class FakeAuthController : FwshController
{
    public FakeAuthController (FwshUser user)
    {
        this.user = user;
    }

    [HttpGet("become-{rolename}")]
    public IActionResult Become (string rolename)
    {
        // Forbid this in Production mode
        if (!env.isDevelopment) { 
            return NotFound (new MessageResult("Not found"));
        }
        if (Enum.TryParse<UserRole>(rolename, true, out UserRole role)) {
            user.ConfirmedRole = role;
            return Ok (new SuccessResult($"Now you are {rolename}"));
        }
        return BadRequest (new MessageResult($"Unrecognized role {rolename}"));
    }
}
