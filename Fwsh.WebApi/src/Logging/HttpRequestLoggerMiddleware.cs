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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

public class HttpRequestLoggerMiddleware
{
    private RequestDelegate next;

    public HttpRequestLoggerMiddleware (RequestDelegate next) 
    {
        this.next = next;
    }

    public async Task InvokeAsync (HttpContext context)
    {
        Console.WriteLine($"\n[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
        Console.WriteLine($"  {context.Request.Method} {context.Request.Path}");
        await next(context);
    }
}
