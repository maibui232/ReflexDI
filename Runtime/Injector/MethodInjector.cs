namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal class MethodInjector : IInjector
    {
        IEnumerable<ReadOnlyParam> IInjector.ResolveParams(IResolver resolver, Type type, object instance)
        {
            var list              = new List<ReadOnlyParam>();
            var injectableMethods = type.GetInjectableMethodInfos();
            foreach (var methodInfo in injectableMethods)
            {
                var parameterValues = new List<object>();
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    var parameterType  = parameterInfo.ParameterType;
                    var parameterValue = resolver.Resolve(parameterType);
                    parameterValues.Add(parameterValue);

                    list.Add(new ReadOnlyParam(parameterType, parameterValue));
                }

                methodInfo.Invoke(instance, parameterValues.ToArray());
            }

            return list;
        }
    }
}