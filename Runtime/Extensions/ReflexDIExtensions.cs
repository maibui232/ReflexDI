namespace ReflexDI
{
    using System;
    using System.Linq;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class ReflexDIExtensions
    {
        private static ReflexDIContainer cachedContainer;

        internal static ReflexDIContainer DIContainer => cachedContainer ??= new ReflexDIContainer();

#region Registration

        public static RegistrationMono WithName(this RegistrationMono registration, string name)
        {
            registration.GameObjectName = name;

            return registration;
        }

        public static Registration WithParameter<T>(this Registration registration, Func<IResolver, T> resolverParam)
        {
            registration.CustomParams.Add(new ReadOnlyParam(typeof(T), resolverParam.Invoke(DIContainer)));

            return registration;
        }

        public static Registration As<T>(this Registration registration)
        {
            ThrowExceptionExtensions.IfTypeNotAssignableFrom(typeof(T), registration.ImplementedType);

            registration.RegisterTypes.Add(typeof(T));

            return registration;
        }

        public static Registration AsSelf(this Registration registration)
        {
            registration.RegisterTypes.Add(registration.ImplementedType);

            return registration;
        }

        public static Registration AsInterfaces(this Registration registration)
        {
            foreach (var type in registration.ImplementedType.GetInterfaces())
            {
                registration.RegisterTypes.Add(type);
            }

            return registration;
        }

        public static Registration AsSelfAndInterfaces(this Registration registration)
        {
            registration.AsSelf();
            registration.AsInterfaces();

            return registration;
        }

        public static Registration NonLazy(this Registration registration)
        {
            registration.RegistrationProvider.IsNonLazy = true;

            return registration;
        }

#endregion

#region Resolver

        public static T Instantiate<T>(this IResolver resolver) where T : class
        {
            return Instantiate(resolver, typeof(T)) as T;
        }

        public static object Instantiate(this IResolver resolver, Type type)
        {
            var constructorParams = resolver.ResolveParams<ConstructorInjector>(type, null);
            var instance          = Activator.CreateInstance(type, constructorParams.Select(param => param.Value).ToArray());

            resolver.Inject(instance);

            return instance;
        }

        public static T InstantiatePrefab<T>(this IResolver resolver, Component prefab, Transform parent = null) where T : MonoBehaviour
        {
            return resolver.InstantiatePrefab(typeof(T), prefab, parent) as T;
        }

        public static object InstantiatePrefab(this IResolver resolver, Type type, Component prefab, Transform parent = null)
        {
            var gameObjectInstance = Object.Instantiate(prefab.gameObject, parent);

            var component = gameObjectInstance.GetComponent(type);
            resolver.Inject(component);

            return component;
        }

        public static void Inject(this IResolver resolver, object instance)
        {
            var type = instance.GetType();
            resolver.ResolveParams<MethodInjector>(type, instance);
            resolver.ResolveParams<FieldInjector>(type, instance);
            resolver.ResolveParams<PropertyInjector>(type, instance);
        }

        public static void InjectGameObject(this IResolver resolver, GameObject gameObject)
        {
            foreach (var monoBehaviour in gameObject.GetComponents<MonoBehaviour>())
            {
                resolver.Inject(monoBehaviour);
            }
        }

#endregion

#region Register

        public static Registration Register<T>(this IBuilder builder)
        {
            return builder.Register(typeof(T));
        }

        public static Registration Register(this IBuilder builder, Type type)
        {
            var registration = new Registration(type);
            builder.AddRegistration(registration);

            return registration;
        }

        public static RegistrationInstance RegisterInstance<T>(this IBuilder builder, T instance)
        {
            return builder.RegisterInstance(typeof(T), instance);
        }

        public static RegistrationInstance RegisterInstance(this IBuilder builder, Type type, object instance)
        {
            ThrowExceptionExtensions.IfInstanceNotAssignableFrom(type, instance);
            var registration = new RegistrationInstance(type, instance);
            builder.AddRegistration(registration);

            return registration;
        }

#endregion

#region Mono Register

        public static RegistrationMonoOnNewPrefab RegisterComponentOnNewPrefab<T>(this IBuilder builder, Component prefab) where T : Component
        {
            return builder.RegisterComponentOnNewPrefab(typeof(T), prefab);
        }

        public static RegistrationMonoOnNewPrefab RegisterComponentOnNewPrefab(this IBuilder builder, Type type, Component prefab)
        {
            var registration = new RegistrationMonoOnNewPrefab(type, prefab);
            builder.AddRegistration(registration);

            return registration;
        }

        public static RegistrationInNewPrefab RegisterComponentInNewPrefab<T>(this IBuilder builder, Component prefab) where T : Component
        {
            return builder.RegisterComponentInNewPrefab(typeof(T), prefab);
        }

        public static RegistrationInNewPrefab RegisterComponentInNewPrefab(this IBuilder builder, Type type, Component prefab)
        {
            ThrowExceptionExtensions.IfPrefabNotAssignableFrom(type, prefab);
            var registration = new RegistrationInNewPrefab(type, prefab);
            builder.AddRegistration(registration);

            return registration;
        }

        public static RegistrationMonoOnNewGameObject RegisterComponentOnNewGameObject<T>(this IBuilder builder) where T : Component
        {
            return builder.RegisterComponentOnNewGameObject(typeof(T));
        }

        public static RegistrationMonoOnNewGameObject RegisterComponentOnNewGameObject(this IBuilder builder, Type type)
        {
            var registration = new RegistrationMonoOnNewGameObject(type);
            builder.AddRegistration(registration);

            return registration;
        }

#endregion
    }
}