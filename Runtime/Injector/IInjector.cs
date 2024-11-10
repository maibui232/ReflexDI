namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal interface IInjector
    {
        internal IEnumerable<IInjectParameter> ResolveParams
        (
            IResolver                                   resolver,
            Type                                        type,
            object                                      instance,
            IReadOnlyDictionary<Type, IInjectParameter> parameters = null
        );
    }
}