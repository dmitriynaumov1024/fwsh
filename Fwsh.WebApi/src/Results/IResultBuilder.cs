namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.WebApi.SillyAuth;

public interface IResultBuilder<out TResult> 
where TResult : class  
{
    TResult Mini();
    TResult ForCustomer();
    TResult ForWorker();
    TResult ForManager();
}

public static class ResultBuilderExtensions 
{
    public static TResult For<TResult> (this IResultBuilder<TResult> result, FwshUser user) 
    where TResult : class 
    {
        return user.ConfirmedRole switch {
            UserRole.Customer => result.ForCustomer(),
            UserRole.Worker => result.ForWorker(),
            UserRole.Manager => result.ForManager(),
            _ => null
        };
    }
}
