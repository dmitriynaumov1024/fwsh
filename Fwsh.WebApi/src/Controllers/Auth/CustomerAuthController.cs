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
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Requests.Auth;
using Fwsh.WebApi.Results;

[ApiController]
[Route("auth/customer")]
public class CustomerAuthController : ControllerBase
{
    private FwshDataContext dataContext;
    private Logger logger;
    private FwshUser user;

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

        var storedCustomer = new Customer() {
            Surname = request.Surname,
            Name = request.Name,
            Patronym = request.Patronym,
            OrgName = request.OrgName,
            IsOrganization = request.OrgName != null && request.OrgName.Length > 1,
            Phone = request.Phone,
            Email = request.Email,
            Password = request.Password.QuickHash()
        };

        bool phoneAlreadyExists = dataContext.Customers
            .Where(c => c.Phone == request.Phone)
            .FirstOrDefault() != null; 

        if (phoneAlreadyExists) {
            return BadRequest(new BadFieldResult("phone"));
        }
        
        try {
            dataContext.Customers.Add(storedCustomer);
            dataContext.SaveChanges();
            int id = storedCustomer.Id;
            return Ok(new CreationResult(id, $"Successfully created {id}"));
        }
        catch (Exception ex) {
            logger.Error(ex.ToString());
        }
        return BadRequest(new MessageResult("Something went wrong"));
    }

    [HttpPost("login")]
    public IActionResult Login (LoginRequest request)
    {
        var storedCustomer = dataContext.Customers
            .Where(c => c.Phone == request.Phone)
            .FirstOrDefault();

        if (storedCustomer == null) {
            return NotFound(new BadFieldResult("phone"));
        }
        if (storedCustomer.Password == request.Password.QuickHash()) {
            user.ConfirmedId = storedCustomer.Id;
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
