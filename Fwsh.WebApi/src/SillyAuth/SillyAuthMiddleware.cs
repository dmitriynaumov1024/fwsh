namespace Fwsh.WebApi.SillyAuth;

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

public class SillyAuthMiddleware
{
    // Note: this middleware is not really responsible for authorization. 
    // It provides session-like infrastructure to persist user info in intervals 
    // between HTTP requests.

    private RequestDelegate next;

    public SillyAuthMiddleware (RequestDelegate next) 
    {
        this.next = next;
    }

    public async Task InvokeAsync (HttpContext context, FwshUser user)
    {
        FwshUser associatedUser = null;
        string key = null;
        bool tokenUpdated = false;

        var token = (string)(context.Request.Headers["Authorization"]);
        if (token != null) {
            var tokens = token.Split(' ');
            if (tokens.Length == 3 && tokens[0] == "Bearer") {
                associatedUser = FwshUserStorage.GetUserByIdToken(tokens[1], tokens[2]);
                if (associatedUser != null) {
                    tokenUpdated = tokens[2] != associatedUser.Token;
                    key = tokens[1];
                }
            }
        }

        // if auth failed
        if (associatedUser == null) {
            var (_key, _user) = FwshUserStorage.NewUser();
            associatedUser = _user;
            key = _key;
            tokenUpdated = true;
        }

        // copy paste eh
        user.ConfirmedId = associatedUser.ConfirmedId;
        user.ConfirmedRole = associatedUser.ConfirmedRole;
        user.Token = associatedUser.Token;

        if (tokenUpdated) {
            Console.WriteLine("  Token updated");
            context.Response.Headers["Authorization"] = $"Bearer {key} {user.Token}";
        }

        await next(context);

        Console.WriteLine($"  Auth token: {key}\n              {user.Token}");

        if (associatedUser.ConfirmedId == user.ConfirmedId) {
            Console.WriteLine($"  ConfirmedId: {user.ConfirmedId}");
        }
        else { 
            // This could mean logging into another account 
            // so we need to reduce TTL of user
            associatedUser.TTL = 0;
            Console.WriteLine($"  ConfirmedId: {associatedUser.ConfirmedId} => {user.ConfirmedId}");
        }

        // is this legal?
        associatedUser.ConfirmedId = user.ConfirmedId;
        associatedUser.ConfirmedRole = user.ConfirmedRole;
        // token should not be mutable by other handlers/controllers
    }
}
