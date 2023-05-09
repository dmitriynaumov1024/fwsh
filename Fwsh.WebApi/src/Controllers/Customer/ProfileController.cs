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
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Requests.Auth;
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

    IActionResult OnUpdate (Customer customer, UpdateRequest<Customer> request)
    {
        try {
            request.ApplyTo(customer);
            dataContext.Customers.Update(customer);
            dataContext.SaveChanges();
            return Ok (new SuccessResult("Successfully updated profile"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError (new FailResult("Something went wrong while trying to update profile"));
        }
    }

    [HttpPost("update")]
    public IActionResult Update (CustomerUpdateRequest request)
    {
        if (request.Validate().State.HasBadFields)
            return BadRequest (new BadFieldResult(request.State.BadFields));

        int id = user.ConfirmedId;
        var customer = dataContext.Customers.Find(id);

        if (customer == null) 
            return NotFound (new MessageResult($"Can not update own profile"));

        if (request.OldPassword.QuickHash() != customer.Password) 
            return BadRequest(new BadFieldResult("oldPassword"));
        
        return OnUpdate(customer, request);
    }

    [HttpPost("set-password")]
    public IActionResult SetPassword (PasswordUpdateRequest request)
    {
        if (request.Validate().State.HasBadFields)
            return BadRequest (new BadFieldResult(request.State.BadFields));

        int id = user.ConfirmedId;
        var customer = dataContext.Customers.Find(id);

        if (customer == null) 
            return NotFound (new MessageResult($"Can not update own profile"));

        if (request.OldPassword.QuickHash() != customer.Password) 
            return BadRequest(new BadFieldResult("oldPassword"));

        return OnUpdate(customer, request);
    }

    [HttpPost("logout")]
    public IActionResult Logout ()
    {
        user.Destroy();

        return Ok (new MessageResult("Successfully logged out"));
    }

}
