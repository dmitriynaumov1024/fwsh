namespace Fwsh.WebApi.Validation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class PropertyValidator
{
    public string Name { get; private set; }
    public object Value { get; private set; }
    public bool IsValid { get; private set; }

    public PropertyValidator (string propertyName, object propertyValue) 
    { 
        this.IsValid = true;
        this.Name = propertyName;
        this.Value = propertyValue;
    }

    public PropertyValidator NotNull()
    {
        if (this.Value == null) 
            this.IsValid = false;

        return this;
    }

    public PropertyValidator Match (Regex regex)
    {
        if (! this.IsValid) return this;

        if (! regex.IsMatch(this.Value as string)) this.IsValid = false;

        return this;
    }

    public PropertyValidator NullOrMatch (Regex regex)
    {
        if (this.IsValid && this.Value is string s)
            if (! regex.IsMatch(this.Value as string)) this.IsValid = false;

        return this;
    }

    public PropertyValidator LengthInRange (int min, int max)
    {
        if (this.IsValid && this.Value is string s)
            if (s.Length < min || s.Length > max) this.IsValid = false;

        return this; 
    }

    public PropertyValidator CountInRange (int min, int max)
    {
        if (this.IsValid && this.Value is ICollection collection)
            if (collection.Count < min || collection.Count > max) this.IsValid = false;

        return this;
    }

    public PropertyValidator ValueInRange (int min, int max)
    {
        if (this.IsValid && this.Value is int number)
            if (number < min || number > max) this.IsValid = false;

        return this;
    }

    public PropertyValidator Condition (bool condition)
    {
        if (! condition) this.IsValid = false;

        return this;
    }

    public PropertyValidator Assert<T> (Func<T, bool> predicate)
    {
        if (this.IsValid && ! predicate((T)this.Value)) this.IsValid = false;

        return this;
    }
}
