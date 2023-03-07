namespace Fwsh.WebApi.Validation;

using System;
using System.Collections.Generic;
using System.Linq;

public class ObjectValidationState
{
    public bool IsValid { get; set; }

    public bool HasBadFields => this.BadFields != null && this.BadFields.Count > 0;

    public ICollection<string> BadFields { get; set; }

    public string Message { get; set; }

    public ObjectValidationState () { }
}
