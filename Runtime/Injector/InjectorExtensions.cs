namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    public static class InjectorExtensions
    {
        private static readonly Dictionary<Type, IInjector> TypeToInjectorCached = new();

        private static IInjector GetInjector<T>() where T : IInjector
        {
            var type = typeof(T);

            if (TypeToInjectorCached.TryGetValue(type, out var injector)) return injector;

            var newInjector = Activator.CreateInstance(type) as IInjector;
            TypeToInjectorCached.Add(type, newInjector);

            return TypeToInjectorCached[type];
        }

        internal static IEnumerable<ReadOnlyParam> ResolveParams<TInjector>(this IResolver resolver, Type type, object instance) where TInjector : IInjector
        {
            return GetInjector<TInjector>().ResolveParams(resolver, type, instance);
        }
    }
}