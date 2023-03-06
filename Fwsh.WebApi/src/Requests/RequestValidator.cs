namespace Fwsh.WebApi.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fwsh.WebApi.Results;

// Simple fluent validation helper
//
public class RequestValidator
{
    private BadFieldResult badResult = new BadFieldResult();
    private SuccessResult goodResult = new SuccessResult();

    public RequestValidator Assert (string propName, bool condition)
    {
        if (! condition) {
            this.badResult.BadFields.Add(propName);
        }
        
        return this;
    }

    public RequestValidator Match (string propName, string propValue, Regex regex)
    {
        if (! regex.IsMatch(propValue)) {
            this.badResult.BadFields.Add(propName);
        }
        
        return this;
    }

    public Result GetVerdict ()
    {
        if (this.badResult.BadFields.Count > 0) {
            return this.badResult;
        }
        else {
            return this.goodResult;
        }
    }
}
