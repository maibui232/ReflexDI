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
                           try
                           {
                               var paramValue = resolver.ResolveTypeWithCustomParams(parameters, parameterType);

                               return new TypeInjectParameter(parameterType, paramValue);
                           }
                           catch (Exception)
                           {
                               throw new ResolveException(type, parameterType);
                           }
                       });
        }
    }
}