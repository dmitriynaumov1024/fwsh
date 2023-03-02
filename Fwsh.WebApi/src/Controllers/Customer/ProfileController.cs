namespace Fwsh.WebApi.Controllers.Customer;

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
using Fwsh.WebApi.Requests.Customer;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.Results.Customer;
using Fwsh.WebApi.SillyAuth;

[ApiController]
[Route("customer/profile")]
public class ProfileController : ControllerBase
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
        var storedCustomer = dataContext.Customers.Find(id);
        
        if (storedCustomer == null) {
            return NotFound(new MessageResult($"Can not view own profile."));
        }

        return Ok (new CustomerProfileResult(storedCustomer)); 
    }

    [HttpPost("update")]
    public IActionResult Update (CustomerUpdateRequest request)
    {
        int id = user.ConfirmedId;
        var storedCustomer = dataContext.Customers.Find(id);

        if (storedCustomer == null) {
            return NotFound(new MessageResult($"Can not update own profile"));
        }
        if (storedCustomer.Password != request.OldPassword.QuickHash()) {
            return BadRequest(new BadFieldResult("oldPassword"));
        }

        storedCustomer.Password = request.NewPassword.QuickHash();
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
