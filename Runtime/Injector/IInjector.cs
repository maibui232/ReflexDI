namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal interface IInjector
    {
        internal IEnumerable<ReadOnlyParam> ResolveParams(IResolver resolver, Type type, object instance);
    }
}