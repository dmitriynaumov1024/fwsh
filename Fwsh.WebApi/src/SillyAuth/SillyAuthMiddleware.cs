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
using Microsoft.Extensions.DependencyInjection;

using Fwsh.WebApi.Logging;

public class SillyAuthMiddleware
{
    // Note: this middleware is not really responsible for authorization. 
    // It provides session-like infrastructure to persist user info in intervals 
    // between HTTP requests.
    private Logger logger;
    private FwshUserStorage userStorage;
    private RequestDelegate next;

    public SillyAuthMiddleware (Logger logger, FwshUserStorage userStorage, RequestDelegate next) 
    {
        this.logger = logger;
        this.userStorage = userStorage;
        this.next = next;
    }

    public async Task InvokeAsync (HttpContext context, FwshUser user)
    {
        FwshUser storedUser = null;
        string key = null;
        bool tokenUpdated = false;

        var token = (string)(context.Request.Headers["Authorization"]);
        if (token != null) {
            var tokens = token.Split(' ');
            if (tokens.Length == 3 && tokens[0] == "Bearer") {
                storedUser = userStorage.GetUserByIdToken(tokens[1], tokens[2]);
                if (storedUser != null) {
                    tokenUpdated = tokens[2] != storedUser.Token;
                    key = tokens[1];
                }
            }
        }

        // if auth failed
        if (storedUser == null) {
            var (_key, _user) = userStorage.NewUser();
            storedUser = _user;
            key = _key;
            tokenUpdated = true;
        }

        // copy paste eh
        user.ConfirmedId = storedUser.ConfirmedId;
        user.ConfirmedRole = storedUser.ConfirmedRole;
        user.Token = storedUser.Token;

        if (tokenUpdated) {
            logger.Warn("Token updated");
            context.Response.Headers["Authorization"] = $"Bearer {key} {user.Token}";
        }

        await next(context);

        logger.Log($"Auth token: {key} {user.Token}");

        if (storedUser.ConfirmedId == user.ConfirmedId) {
            logger.Log($"ConfirmedId: {user.ConfirmedId}");
        }
        else { 
            // This could mean logging into another account 
            // so we need to reduce TTL of user
            storedUser.TTL = 0;
            logger.Warn($"ConfirmedId: {storedUser.ConfirmedId} => {user.ConfirmedId}");
        }

        storedUser.ConfirmedId = user.ConfirmedId;
        storedUser.ConfirmedRole = user.ConfirmedRole;
        // token should not be mutable by other handlers/controllers
        userStorage.UpdateUser(key, storedUser);
    }
}
