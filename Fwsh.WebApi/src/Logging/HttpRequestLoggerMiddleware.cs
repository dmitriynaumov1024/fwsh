namespace Fwsh.WebApi.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Fwsh.Logging;

public class HttpRequestLoggerMiddleware
{
    private Logger logger;
    private RequestDelegate next;

    public HttpRequestLoggerMiddleware (Logger logger, RequestDelegate next) 
    {
        this.logger = logger;
        this.next = next;
    }

    public async Task InvokeAsync (HttpContext context)
    {
        logger.Log($"{context.Request.Method} {context.Request.Path}");
        await next(context);
    }
}
