namespace Fwsh.WebApi.Controllers.Auth;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Fwsh.Utils;
using Fwsh.WebApi.SillyAuth; 
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/fake-auth")]
public class FakeAuthController : ControllerBase
{
    private FwshUser user;

    public FakeAuthController (FwshUser user)
    {
        this.user = user;
    }

    [HttpGet("become-{rolename}")]
    public IActionResult Become (string rolename)
    {
        if (!env.isDevelopment) { 
            return NotFound (new MessageResult("Not found"));
        }
        if (Enum.TryParse<UserRole>(rolename, true, out UserRole role)) {
            user.ConfirmedRole = role;
            return Ok (new MessageResult($"Now you are {rolename}"));
        }
        return BadRequest (new MessageResult($"Unrecognized role {rolename}"));
    }
}
