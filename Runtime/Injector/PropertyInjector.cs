namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal class PropertyInjector : IInjector
    {
        IEnumerable<ReadOnlyParam> IInjector.ResolveParams(IResolver resolver, Type type, object instance)
        {
            foreach (var propertyInfo in type.GetInjectablePropertyInfos())
            {
                var propertyType  = propertyInfo.PropertyType;
                var propertyValue = resolver.Resolve(propertyType);

                if (!propertyInfo.CanWrite)
                {
                    throw new Exception($"Property {propertyInfo.Name} has no setter");
                }

                propertyInfo.SetValue(instance, propertyValue);

                yield return new ReadOnlyParam(propertyInfo.PropertyType, propertyValue);
            }
        }
    }
}