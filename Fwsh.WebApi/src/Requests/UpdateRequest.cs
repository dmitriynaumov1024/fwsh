namespace Fwsh.WebApi.Requests;

using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public interface UpdateRequest<TEntity>
where TEntity : class
{
    void ApplyTo (TEntity entity);
}
