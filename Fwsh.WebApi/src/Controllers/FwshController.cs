namespace Fwsh.WebApi.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Requests.Customer;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

// This is to be base class for all Fwsh WebAPI controllers 
//
public class FwshController : ControllerBase
{
    protected FwshDataContext dataContext;
    protected Logger logger;
    protected FwshUser user;

    public ObjectResult ServerError<T> (T data)
    {
        return new ObjectResult(data) {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    public ObjectResult Found<T> (T data)
    {
        return new ObjectResult(data) {
            StatusCode = StatusCodes.Status302Found
        };
    }
}

