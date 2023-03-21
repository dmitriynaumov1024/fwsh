namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;

public class BadFieldResult : FailResult
{
    public List<string> BadFields { get; set; }

    public BadFieldResult () 
    { 
        this.BadFields = new List<string>();
    }

    public BadFieldResult (params string[] fields) 
    {
        this.BadFields = new List<string>(fields.Length);
        foreach (string field in fields) {
            this.BadFields.Add(field);
        }
    }

    public BadFieldResult (ICollection<string> fields) 
    {
        this.BadFields = new List<string>(fields.Count);
        foreach (string field in fields) {
            this.BadFields.Add(field);
        }
    }
}
