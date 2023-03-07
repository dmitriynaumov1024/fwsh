namespace Fwsh.WebApi.Requests;

using Fwsh.WebApi.Results;
using Fwsh.WebApi.Validation;

public abstract class Request
{
    protected abstract void OnValidation (ObjectValidator validator);

    public ObjectValidationState State { get; private set; }

    public Request Validate()
    {
        var validator = new ObjectValidator();
        this.OnValidation(validator);
        this.State = validator.GetVerdict();
        return this;
    }
}
