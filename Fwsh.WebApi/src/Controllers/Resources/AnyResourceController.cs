namespace Fwsh.WebApi.Controllers.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("resources/any")]
public class AnyResourceController : ResourceController
{
    protected override IQueryable<Resource> dbQueryableSet => 
        dataContext.Resources
            .Include(r => r.Stored)
            .Include(r => r.Stored.Supplier);

    public AnyResourceController (FwshDataContext dataContext, Logger logger, FwshUser user)
    : base (dataContext, logger, user) { }

    // should include list and view actions from base class 
}
