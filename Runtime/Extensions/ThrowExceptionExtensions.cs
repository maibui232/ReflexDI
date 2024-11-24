namespace ReflexDI
{
    using System;
    using UnityEngine;

    public static class ThrowExceptionExtensions
    {
        internal static void IfInstanceNotAssignableFrom(Type baseType, object instance)
        {
            if (!baseType.IsAssignableFrom(instance.GetType())) throw new ArgumentException($"Instance type {instance.GetType()} is not assignable to type {baseType}");
        }

        internal static void IfPrefabNotAssignableFrom(Type type, Component prefab)
        {
            if (!prefab.TryGetComponent(type, out _)) throw new ArgumentException($"Prefab type {prefab.GetType()} is not assignable to type {type}");
        }

        internal static void IfTypeNotAssignableFrom(Type baseType, Type type)
        {
            if (!baseType.IsAssignableFrom(type)) throw new ArgumentException($"Instance type {baseType} is not assignable to type {type}");
        }

        internal static void HasRegistration(Type type)
        {
            throw new ArgumentException($"Type {type} has been registered yet");
        }

        internal static void HasNotRegistration(Type type)
        {
            throw new ArgumentException($"Type {type} has not been registered yet");
        }
    }
}