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

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.Logging;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.RouteGuard;

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

    public void Configure (IApplicationBuilder app, IWebHostEnvironment environment)
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

        // Custom silly authentication
        app.UseMiddleware<SillyAuthMiddleware>();

        // Custom route guard
        app.UseMiddleware<RouteGuardMiddleware>();

        // Configure endpoints
        app.UseEndpoints (endpoints => {
            endpoints.MapControllers();
        });

        app.Run (async (context) => {
            var request = context.Request;
            var response = context.Response;
            
            response.StatusCode = StatusCodes.Status404NotFound;
            
            if (env.isDevelopment) {
                await response.WriteAsJsonAsync (new {
                    Message = $"Nothing here on {request.Path}",
                    Host = request.Host.ToString(),
                    Method = request.Method.ToString(),
                    Path = request.Path.ToString(),
                    Query = String.Join(", ", request.Query.Select(kv => kv.Key+"="+kv.Value))
                });
            }
            else {
                await response.WriteAsJsonAsync<object>(null);
            }
        });
    }
}
