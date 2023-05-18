namespace Fwsh.WebApi.Utils;

using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static void UseStaticFiles (
        this IApplicationBuilder app, 
        string requestPath, 
        string localPath )
    {
        string pathRoot = Path.Combine (Directory.GetCurrentDirectory(), localPath);
        if (!Directory.Exists(pathRoot)) {
            Directory.CreateDirectory(pathRoot);
        }
        app.UseStaticFiles ( new StaticFileOptions {
            FileProvider = new PhysicalFileProvider (pathRoot),
            RequestPath = requestPath
        });
    }
    
}
