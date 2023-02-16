namespace Fwsh.WebApi.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Fwsh.Utils;
using Fwsh.WebApi.SillyAuth; 

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
            return NotFound (new {
                Message = "Not found"
            });
        }
        if (Enum.TryParse<UserRole>(rolename, true, out UserRole role)) {
            user.ConfirmedRole = role;
            return Ok (new {
                Message = $"Now you are {rolename}"
            });
        }
        return BadRequest (new { 
            Message = $"Unrecognized role {rolename}"
        });
    }
}
