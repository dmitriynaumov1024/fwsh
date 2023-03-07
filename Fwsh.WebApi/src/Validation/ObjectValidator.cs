namespace Fwsh.WebApi.Validation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// Simple fluent validation helper
//
public class ObjectValidator
{
    private List<string> badFields = new List<string>();

    private List<PropertyValidator> propertyValidators = new List<PropertyValidator>();

    public PropertyValidator Property (string propName, object propValue)
    {
        var result = new PropertyValidator(propName, propValue);
        this.propertyValidators.Add(result);
        return result;
    }

    public ObjectValidator Assert (string propName, bool condition)
    {
        if (! condition) {
            this.badFields.Add(propName);
        }
        
        return this;
    }

    public ObjectValidator Match (string propName, string propValue, Regex regex)
    {
        if (! regex.IsMatch(propValue)) {
            this.badFields.Add(propName);
        }
        
        return this;
    }

    public ObjectValidator Pass ()
    {
        return this;
    }

    public void DoNothing()
    {
        return;    
    }

    public ObjectValidationState GetVerdict ()
    {
        SortedSet<string> badFieldSet = new SortedSet<string> (
            this.propertyValidators
                .Where(p => !p.IsValid)
                .Select(p => p.Name)
                .Concat(this.badFields)
        );

        return new ObjectValidationState {
            IsValid = this.badFields.Count == 0,
            BadFields = badFieldSet
        };
    }
}
