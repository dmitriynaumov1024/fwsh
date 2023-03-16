namespace Fwsh.WebApi.RouteGuard;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Fwsh.WebApi.SillyAuth;
using Fwsh.Logging;

public class RouteGuardMiddleware
{
    private Logger logger;
    private RequestDelegate next;

    public RouteGuardMiddleware (Logger logger, RequestDelegate next) 
    {
        this.logger = logger;
        this.next = next;
    }

    public async Task InvokeAsync (HttpContext context, FwshUser user)
    {
        string path = context.Request.Path.ToString()
            .Split('/').FirstOrDefault(s => s.Length > 0);
        UserRole role = user.ConfirmedRole;

        bool allowed = (path == "auth") 
            || (path == "catalog") 
            || (path == "customer" && role == UserRole.Customer) 
            || (path == "worker" && role == UserRole.Worker)
            || (path == "manager" && role == UserRole.Manager)
            || (path == "resources" && role >= UserRole.Worker)
            || (path == "admin" && role == UserRole.Root);

        if (allowed) {
            await next(context);
        }
        else {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new {
                Message = "Should be authorized to access " + context.Request.Path.ToString()
            });
        }
    }
}
