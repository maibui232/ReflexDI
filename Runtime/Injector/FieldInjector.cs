namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    internal class FieldInjector : IInjector
    {
        IEnumerable<ReadOnlyParam> IInjector.ResolveParams(IResolver resolver, Type type, object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(type.FullName);
            }

            var fieldInfos = type.GetInjectableFieldInfos();

            foreach (var fieldInfo in fieldInfos)
            {
                var fieldType  = fieldInfo.FieldType;
                var fieldValue = resolver.Resolve(fieldType);

                fieldInfo.SetValue(instance, fieldValue);

                yield return new ReadOnlyParam(fieldType, fieldValue);
            }
        }
    }
}