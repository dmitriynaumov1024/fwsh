namespace Fwsh.WebApi.Results;

using System;

public class TryLaterResult : FailResult
{
    public DateTime TryAgainAt { get; set; }

    public TryLaterResult () { }

    public TryLaterResult (DateTime tryAgainAt, string message = null) 
    {
        this.TryAgainAt = tryAgainAt;
        this.Message = message;
    }
}
