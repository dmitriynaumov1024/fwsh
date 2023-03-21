namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

public class ListResult<TItem> 
{
    public IList<TItem> Items { get; set; }

    public ListResult () { }

    public ListResult (IEnumerable<TItem> items) 
    {
        this.Items = items.ToList();
    }
}
