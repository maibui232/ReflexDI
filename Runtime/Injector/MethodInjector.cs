namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal class MethodInjector : IInjector
    {
        IEnumerable<IInjectParameter> IInjector.ResolveParams
        (
            IResolver                                   resolver,
            Type                                        type,
            object                                      instance,
            IReadOnlyDictionary<Type, IInjectParameter> parameters
        )
        {
            var list              = new List<TypeInjectParameter>();
            var injectableMethods = type.GetInjectableMethodInfos();
            foreach (var methodInfo in injectableMethods)
            {
                var parameterValues = new List<object>();
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    var parameterType = parameterInfo.ParameterType;
                    var parameterValue = parameters.TryGetValue(parameterType, out var injectParameter)
                                             ? injectParameter.Value
                                             : resolver.Resolve(parameterType);
                    parameterValues.Add(parameterValue);

                    list.Add(new TypeInjectParameter(parameterType, parameterValue));
                }

                methodInfo.Invoke(instance, parameterValues.ToArray());
            }

            return list;
        }
    }
}