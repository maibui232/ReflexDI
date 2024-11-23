namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal class FieldInjector : IInjector
    {
        IEnumerable<IInjectParameter> IInjector.ResolveParams
        (
            IResolver                                   resolver,
            Type                                        type,
            object                                      instance,
            IReadOnlyDictionary<Type, IInjectParameter> parameters
        )
        {
            if (instance == null) throw new ArgumentNullException(type.FullName);

            var fieldInfos = type.GetInjectableFieldInfos();

            foreach (var fieldInfo in fieldInfos)
            {
                var fieldType = fieldInfo.FieldType;
                var fieldValue = parameters.TryGetValue(fieldType, out var injectParam)
                                     ? injectParam.Value
                                     : resolver.Resolve(fieldType);

                fieldInfo.SetValue(instance, fieldValue);

                yield return new TypeInjectParameter(fieldType, fieldValue);
            }
        }
    }
}