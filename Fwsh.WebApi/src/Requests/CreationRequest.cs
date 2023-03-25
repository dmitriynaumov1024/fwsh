namespace Fwsh.WebApi.Requests;

using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public interface CreationRequest<TEntity>
where TEntity : class
{
    TEntity Create();
}
