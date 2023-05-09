namespace Fwsh.WebApi.Requests;

using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public interface UpdateRequest<in TEntity>
where TEntity : class
{
    void ApplyTo (TEntity entity);
}
