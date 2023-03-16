namespace Fwsh.WebApi.Requests;

using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class UpdateRequest<TEntity> : Request 
where TEntity : class
{
    public abstract void ApplyTo (TEntity entity);
}
