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

public class FwshController : ControllerBase
{
    public ObjectResult ServerError<T> (T data)
    {
        return new ObjectResult(data) {
            StatusCode = 500
        };
    }
}

