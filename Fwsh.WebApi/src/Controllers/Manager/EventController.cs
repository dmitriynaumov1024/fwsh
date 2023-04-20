namespace Fwsh.WebApi.Controllers.Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Controllers;
using Fwsh.WebApi.Results;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Utils;

[ApiController]
[Route("manager/events")]
public class EventController : FwshController
{
    public EventController (FwshDataContext dataContext, Logger logger, FwshUser user)
    {
        this.dataContext = dataContext;
        this.logger = logger;
        this.user = user;
    }

    [HttpGet("{eventType}/list")]
    public IActionResult List (string eventType = null, string fromDate = null, string toDate = null, string groupBy = null)
    {
        DateTime from, to;

        if (! DateTime.TryParse(fromDate, out from))
            return BadRequest(new BadFieldResult("fromDate"));

        if (! DateTime.TryParse(toDate, out to)) 
            return BadRequest(new BadFieldResult("toDate"));

        return eventType.ToLower() switch {
            "signup" => ListSignupEvents(from, to, groupBy),
            _ => BadRequest(new BadFieldResult("eventType"))
        };
    }

    public IActionResult ListSignupEvents (DateTime fromDate, DateTime toDate, string groupBy)
    {
        groupBy = groupBy?.ToLower();

        bool validGroupBy = groupBy == "year" 
                         || groupBy == "month" 
                         || groupBy == "day";

        if (! validGroupBy) {
            return BadRequest(new BadFieldResult("groupBy"));
        }

        var events = dataContext.SignupEvents;

        var groupedEvents = groupBy switch {
            "year" => events.GroupBy(e => e.EventYear),
            "month" => events.GroupBy(e => e.EventYear*12 + e.EventMonth),
            "day" => events.GroupBy(e => e.EventYear*400 + e.EventMonth*31 + e.EventDay),
            _ => events.GroupBy(e => 1)
        };

        return Ok ( groupedEvents.Select(g => new {
            Event = g.FirstOrDefault(),
            Count = g.Count()
        }) );
    }

}
