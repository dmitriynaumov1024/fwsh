namespace Fwsh.WebApi.Results;

using System;
using System.Collections.Generic;
using System.Linq;

public class PaginationResult<TItem> : ListResult<TItem> 
{
    public int? Page { get; set; }
    public int? Previous { get; set; }
    public int? Next { get; set; }
    // Items inherited from ListResult 

    public static PaginationResult<TItem> Empty()
    {
        return new PaginationResult<TItem>() {
            Page = 0,
            Previous = null,
            Next = null
        };
    }
}
