namespace Fwsh.WebApi.Controllers.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.FileStorage;
using Fwsh.WebApi.Requests;
using Fwsh.WebApi.Requests.Manager;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/priceformation")]
public class PriceFormationController : FwshController
{
    public PriceFormationController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpPost("update-default-prices")]
    public IActionResult UpdateDefaultPrices ()
    {
        double avgDecorPrice = dataContext.Resources
            .Where(r => r.SlotName == SlotNames.Decor)
            .Average(r => r.PricePerUnit);

        double avgFabricPrice = dataContext.Resources
            .Where(r => r.SlotName == SlotNames.Fabric)
            .Average(r => r.PricePerUnit);

        PriceFormation.DefaultDecorPrice = Math.Ceiling(avgDecorPrice);
        PriceFormation.DefaultFabricPrice = Math.Ceiling(avgFabricPrice);

        logger.Log("PriceFormation\n  DefaultDecorPrice: {0} \n  DefaultFabricPrice: {1}", 
            PriceFormation.DefaultDecorPrice, PriceFormation.DefaultFabricPrice);

        return Ok(new SuccessResult("Successfully updated default resource prices"));
    }

}
