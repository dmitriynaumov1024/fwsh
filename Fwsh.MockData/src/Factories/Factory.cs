namespace Fwsh.MockData;

public abstract class Factory<T>
{
    public abstract T Next();
    public virtual int? FixedSize => null; 
    public virtual T[] All() => null;
}
