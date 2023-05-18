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
using Microsoft.EntityFrameworkCore;

using Fwsh.Utils;
using Fwsh.Common;
using Fwsh.Database;
using Fwsh.Logging;
using Fwsh.WebApi.FileStorage;
using Fwsh.WebApi.Logging;
using Fwsh.WebApi.SillyAuth;
using Fwsh.WebApi.RouteGuard;
using Fwsh.WebApi.Utils;

public class Startup 
{
    public void ConfigureServices (IServiceCollection services)
    {
        services.AddSingleton<Logger, ConsoleLogger>();

        if (env.get("DB_HOST") == "memory") 
            services.AddDbContext<FwshDataContext, FwshDataContextInMemory>(ServiceLifetime.Scoped);
        else 
            services.AddDbContext<FwshDataContext, FwshDataContextPostgres>(ServiceLifetime.Scoped);

        services.AddSingleton<FileStorageProvider>(new PhysicalFileStorageProvider(env.get("UPLOAD_DIR")));

        services.AddSingleton<FwshUserStorage, FwshUserStorageInMemory>();

        services.AddScoped<FwshUser>();

        services.AddRouting();

        services.AddControllers().AddControllersAsServices();

        this.DoStartupRoutine(services);
    }

    public void DoStartupRoutine (IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var serviceScope = serviceProvider.CreateScope();
        var dataContext = serviceProvider.GetService<FwshDataContext>();
        var logger = serviceProvider.GetService<Logger>();

        // create first manager account if there is nothing yet
        bool managerExists = dataContext.Workers
            .Where(E.Worker.IsManager)
            .Count() > 0;

        if (! managerExists) {
            var manager = new Worker() {
                Name = "Default Manager",
                Phone = env.get("MGR_LOGIN") ?? "admin",
                Password = env.get("MGR_PASSWORD").QuickHash(),
                Roles = new[] { WorkerRoles.Management }
            };
            dataContext.Workers.Add(manager);
            dataContext.SaveChanges();
            logger.Log("Created default manager");
        }

        // recalculate resource prices
        var priceController = serviceProvider
            .GetService<Fwsh.WebApi.Controllers.Manager.PriceFormationController>();
        priceController.UpdateDefaultPrices();

        // recalculate design prices
        var designController = serviceProvider
            .GetService<Fwsh.WebApi.Controllers.Manager.DesignController>();
        designController.Recalculate();

        serviceScope.Dispose();
        serviceProvider.Dispose();
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

        // Console logging
        app.UseMiddleware<HttpRequestLoggerMiddleware>();

        if (env.isDevelopment) {
            app.UseStaticFiles ("/files", env.get("UPLOAD_DIR"));
        }

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
