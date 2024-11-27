namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal class PropertyInjector : IInjector
    {
        IEnumerable<IInjectParameter> IInjector.ResolveParams
        (
            IResolver                                   resolver,
            Type                                        type,
            object                                      instance,
            IReadOnlyDictionary<Type, IInjectParameter> parameters
        )
        {
            foreach (var propertyInfo in type.GetInjectablePropertyInfos())
            {
                var propertyType  = propertyInfo.PropertyType;
                var propertyValue = resolver.ResolveTypeWithCustomParams(parameters, propertyType);

                if (!propertyInfo.CanWrite) throw new Exception($"Property {propertyInfo.Name} has no setter");

                propertyInfo.SetValue(instance, propertyValue);

                yield return new TypeInjectParameter(propertyInfo.PropertyType, propertyValue);
            }
        }
    }
}