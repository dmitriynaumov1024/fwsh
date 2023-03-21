namespace Fwsh.WebApi.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using Fwsh.WebApi.Results;

public static class QueryableExtensions
{
    // Pages start from 0 (zero)
    public static PaginationResult<TSource> Paginate<TSource> (
        this IQueryable<TSource> dataset, int page, int pageSize )
    {
        var items = dataset
            .Skip(page * pageSize)
            .Take(pageSize + 1)
            .ToList();

        bool hasPrevious = page > 0, 
            hasNext = items.Count == pageSize + 1,
            hasItems = items.Count > 0;
        
        return new PaginationResult<TSource>() {
            Page = page,
            Previous = hasPrevious ? (page - 1) : null,
            Next = hasNext ? (page + 1) : null,
            Items = items.Take(pageSize).ToList()
        };
    }

    // Pages start from 0 (zero)
    public static PaginationResult<TResult> Paginate<TSource, TResult> (
        this IQueryable<TSource> dataset, int page, int pageSize, 
        Func<TSource, TResult> resultSelector )
    {
        var items = dataset
            .Skip(page * pageSize)
            .Take(pageSize + 1)
            .ToList();

        bool hasPrevious = page > 0, 
            hasNext = items.Count == pageSize + 1,
            hasItems = items.Count > 0;
        
        return new PaginationResult<TResult>() {
            Page = page,
            Previous = hasPrevious ? (page - 1) : null,
            Next = hasNext ? (page + 1) : null,
            Items = items.Take(pageSize).Select(resultSelector).ToList()
        };
    }

    public static ListResult<TSource> Listiate<TSource> (
        this IQueryable<TSource> dataset, int maxSize )
    {
        return new ListResult<TSource> (
            dataset.Take(maxSize)
        );
    }

    public static ListResult<TResult> Listiate<TSource, TResult> (
        this IQueryable<TSource> dataset, int maxSize, 
        Func<TSource, TResult> resultSelector )
    {
        return new ListResult<TResult> (
            dataset.Take(maxSize).Select(resultSelector)
        );
    }
}
