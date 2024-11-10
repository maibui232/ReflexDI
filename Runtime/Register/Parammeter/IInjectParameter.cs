namespace ReflexDI
{
    using System;

    public interface IInjectParameter
    {
        Type   ParameterType { get; }
        object Value         { get; }
    }
}