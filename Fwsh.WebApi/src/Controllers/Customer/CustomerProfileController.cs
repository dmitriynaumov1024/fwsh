namespace Fwsh.WebApi.Controllers;

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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;

[ApiController]
[Route("customer/profile")]
public class CustomerProfileController : ControllerBase
{
    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

    public CustomerProfileController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("view")]
    public IActionResult View ()
    {
        int id = user.ConfirmedId;
        var storedCustomer = dataContext.Customers.Find(id);
        
        if (storedCustomer == null) {
            return NotFound(new MessageResult($"Can not view own profile."));
        }

        return Ok (new CustomerResult(storedCustomer)); 
    }

    [HttpPost("update")]
    public IActionResult Update (CustomerUpdateRequest request)
    {
        int id = user.ConfirmedId;
        var storedCustomer = dataContext.Customers.Find(id);

        if (storedCustomer == null) {
            return NotFound(new MessageResult($"Can not update own profile"));
        }
        if (storedCustomer.Password != request.OldPassword.SHA512Hash()) {
            return BadRequest(new BadFieldResult("oldPassword"));
        }

        storedCustomer.Password = request.NewPassword.SHA512Hash();
        dataContext.Customers.Update(storedCustomer);
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
