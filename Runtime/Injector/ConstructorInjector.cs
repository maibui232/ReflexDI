namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ConstructorInjector : IInjector
    {
        IEnumerable<IInjectParameter> IInjector.ResolveParams
        (
            IResolver                                   resolver,
            Type                                        type,
            object                                      instance,
            IReadOnlyDictionary<Type, IInjectParameter> parameters
        )
        {
            var constructorInfo = type.GetSingleConstructorInfo();

            return constructorInfo
               .GetParameters()
               .Select(parameterInfo => parameterInfo.ParameterType)
               .Select(parameterType =>
                       {
                           var paramValue = parameters.TryGetValue(parameterType, out var parameter)
                                                ? parameter.Value
                                                : resolver.Resolve(parameterType);

                           return new TypeInjectParameter(parameterType, paramValue);
                       });
        }
    }
}