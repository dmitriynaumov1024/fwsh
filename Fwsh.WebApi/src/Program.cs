namespace Fwsh.WebApi;

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

using Fwsh.Utils; 

public class Program 
{
    public static void Main (string[] args)
    {
        var app = new WebHostBuilder()
            .UseEnvironment(env.get("ENVIRONMENT_TYPE"))
            .UseKestrel()
            .UseStartup<Startup>()
            .UseUrls("http://0.0.0.0:4000")
            .Build();

        app.Run();
    }
}