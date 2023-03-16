namespace Fwsh.WebApi.Requests;

using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class CreationRequest<TEntity> : Request 
where TEntity : class
{
    public abstract TEntity Create();
}
