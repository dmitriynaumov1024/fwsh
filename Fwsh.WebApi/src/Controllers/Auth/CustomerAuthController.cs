namespace Fwsh.WebApi.Controllers.Auth;

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
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Requests.Auth;
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/customer")]
public class CustomerAuthController : FwshController
{
    public CustomerAuthController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpPost("signup")]
    public IActionResult Signup (CustomerSignupRequest request)
    {
        if (request.Validate().State.HasBadFields) {
            return BadRequest(new BadFieldResult(request.State.BadFields));
        }
        if (! request.State.IsValid) {
            return BadRequest(new MessageResult(request.State.Message ?? "Something went wrong"));
        }

        bool phoneAlreadyExists = dataContext.Customers
            .Where(c => c.Phone == request.Phone)
            .FirstOrDefault() != null; 

        if (phoneAlreadyExists) {
            return BadRequest(new BadFieldResult("phone"));
        }
        
        try {
            var customer = request.Create();
            dataContext.Customers.Add(customer);
            dataContext.SaveChanges();
            int id = customer.Id;
            return Ok(new CreationResult(id, $"Successfully created {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
            return ServerError(new FailResult("Something went wrong"));
        }
    }

    [HttpPost("login")]
    public IActionResult Login (LoginRequest request)
    {
        var customer = dataContext.Customers
            .FirstOrDefault(c => c.Phone == request.Phone);

        if (customer == null) {
            return NotFound(new BadFieldResult("phone"));
        }
        if (customer.Password == request.Password.QuickHash()) {
            user.ConfirmedId = customer.Id;
            user.ConfirmedRole = UserRole.Customer;
            return Ok(new SuccessResult("Successfully logged in"));
        }
        return BadRequest(new BadFieldResult("password"));
    }

    [HttpPost("logout")]
    public IActionResult Logout ()
    {
        user.Destroy();

        return Ok(new MessageResult("Successfully logged out"));
    }

}
