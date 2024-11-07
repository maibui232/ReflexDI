namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ConstructorInjector : IInjector
    {
        IEnumerable<ReadOnlyParam> IInjector.ResolveParams(IResolver resolver, Type type, object instance)
        {
            var constructorInfo = type.GetSingleConstructorInfo();

            return constructorInfo
               .GetParameters()
               .Select(parameterInfo => parameterInfo.ParameterType)
               .Select(parameterType => new ReadOnlyParam(parameterType, resolver.Resolve(parameterType)));
        }
    }
}