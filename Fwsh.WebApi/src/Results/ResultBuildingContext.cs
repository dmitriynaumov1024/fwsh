namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;

// To solve various result building issues, like reference loops
//
public class ResultBuildingContext
{
    public int Depth { get; private set; } = 0;

    public void Push() 
    { 
        Depth += 1;
    }

    public void Pop() 
    {
        if (Depth > 0) Depth -= 1;
    }

    private HashSet<object> objectSet = new HashSet<object>();

    public void Add (object obj) 
    {
        objectSet.Add(obj);
    }

    public bool Contains (object obj) 
    {
        return objectSet.Contains(obj);
    }
}
