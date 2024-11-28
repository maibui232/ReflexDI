namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        internal static ConstructorInfo GetSingleConstructorInfo(this Type type, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var ctors = type.GetConstructors(bindingFlags);

            if (ctors.Length != 1) throw new Exception($"There must be exactly one constructor with the name {type.Name}");

            return ctors[0];
        }

        public static FieldInfo[] GetInjectableFieldInfos(this Type type, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return type.GetFields(bindingFlags).Where(fieldInfo => fieldInfo.GetCustomAttribute<InjectableAttribute>() != null).ToArray();
        }

        public static PropertyInfo[] GetInjectablePropertyInfos(this Type type, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return type.GetProperties(bindingFlags).Where(propInfo => propInfo.GetCustomAttribute<InjectableAttribute>() != null).ToArray();
        }

        public static MethodInfo[] GetInjectableMethodInfos(this Type type, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            return type.GetMethods(bindingFlags).Where(methodInfo => methodInfo.GetCustomAttribute<InjectableAttribute>() != null).ToArray();
        }

        internal static IEnumerable<Type> GetDerivedTypes(this Type baseType)
        {
            var result     = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                result.AddRange(types.Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract));
            }

            return result;
        }
    }
}