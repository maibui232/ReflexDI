namespace ReflexDI
{
    using System;

    public interface IResolver
    {
        T      Resolve<T>();
        object Resolve(Type type);
    }
}