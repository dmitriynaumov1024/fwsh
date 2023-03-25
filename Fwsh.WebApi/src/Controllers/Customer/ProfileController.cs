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
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.Requests.Customer;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;

[ApiController]
[Route("customer/profile")]
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
        var customer = dataContext.Customers.Find(id);
        
        if (customer == null) {
            return NotFound (new MessageResult($"Can not view own profile."));
        }

        return Ok (new CustomerResult(customer)); 
    }

    [HttpPost("update")]
    public IActionResult Update (CustomerUpdateRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest (new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest (new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        int id = user.ConfirmedId;
        var customer = dataContext.Customers.Find(id);

        if (customer == null) {
            return NotFound (new MessageResult($"Can not update own profile"));
        }
        if (customer.Password != request.OldPassword.QuickHash()) {
            return BadRequest (new BadFieldResult("oldPassword"));
        }

        try {
            request.ApplyTo(customer);
            dataContext.Customers.Update(customer);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Profile updated successfully"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to update profile"));
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout ()
    {
        user.Destroy();

        return Ok (new MessageResult("Successfully logged out"));
    }

}
