namespace Fwsh.WebApi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Fwsh.Common;
using Fwsh.Database;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.Logging;

public class Startup 
{
    public void ConfigureServices (IServiceCollection services)
    {
        services.AddSingleton<Logger, ConsoleLogger>();
        services.AddSingleton<FwshDataContext, FwshDataContextPostgres>();
        services.AddSingleton<FwshUserStorage, FwshUserStorageInMemory>();
        // services.AddSingleton<UniformCrudProvider>();
        services.AddRouting();
        services.AddControllers();

        // services.AddDistributedMemoryCache();
        // services.AddSession();

        services.AddScoped<FwshUser>();
    }

    public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Enable routing
        app.UseRouting();

        // Enable CORS
        app.UseCors (policy => {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Authorization");
        });

        // Enable session
        // app.UseSession();

        // Console logging
        app.UseMiddleware<HttpRequestLoggerMiddleware>();

        // Custom silly auth
        app.UseMiddleware<SillyAuthMiddleware>();

        // Configure endpoints
        app.UseEndpoints (endpoints => {
            endpoints.MapControllers();
        });

        app.Run (async (context) => {
            var request = context.Request;
            var response = context.Response;
            var query = String.Join("\n", request.Query.Select(kv => kv.Key+"="+kv.Value));
            response.StatusCode = 404;
            response.ContentType = "text/plain; encoding=utf-8";
            await response.WriteAsync (
                $"Nothing here\n" +
                $"Host: {request.Host}\n" +
                $"Method: {request.Method}\n" +
                $"Path: {request.Path}\n" +
                $"Query: {query}\n"
            );
        });
    }
}
