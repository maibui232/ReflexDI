namespace ReflexDI
{
    using System;
    using System.Linq;
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
    }

    internal class RootGameScopeMissingException : Exception
    {
        public RootGameScopeMissingException()
        {
            throw new Exception("Root Game Scope is missing");
        }
    }

    internal class ResolveException : Exception
    {
        public ResolveException(Type concreteType, params Type[] paramTypes)
        {
            var paramNameTypes = paramTypes.Select(paramType => paramType.FullName).ToArray();
            
            throw new Exception("Could not resolve " + string.Join(",\n", paramNameTypes) + " to " + concreteType.FullName);
        }
    }

internal class RegistrationExistException : Exception
    {
        public RegistrationExistException(Type type)
        {
            throw new Exception($"Type {type} has been registered yet");
        }
    }

    internal class RegistrationNotExistException : Exception
    {
        public RegistrationNotExistException(Type type)
        {
            throw new Exception($"Type {type} has not been registered yet");
        }
    }
}