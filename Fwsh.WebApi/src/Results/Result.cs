namespace Fwsh.WebApi.Results;

using System;

public abstract class Result
{
    public static explicit operator Result (bool success) 
    {
        if (success) return new SuccessResult();
        else return new FailResult();
    } 
}
