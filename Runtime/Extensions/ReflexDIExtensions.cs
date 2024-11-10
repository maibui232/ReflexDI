namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
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

        public static Registration WithParameter(this Registration registration, object value)
        {
            var valueType = value.GetType();
            registration.CustomParameters.Add(valueType, new TypeInjectParameter(valueType, value));

            return registration;
        }

        public static Registration WithParameter(this Registration registration, Type type, Func<IResolver, object> value)
        {
            registration.CustomParameters.Add(type, new FuncInjectParameter(type, DIContainer, _ => value));

            return registration;
        }

        public static Registration WithParameter<T>(this Registration registration, Func<IResolver, T> value)
        {
            registration.CustomParameters.Add(typeof(T), new FuncInjectParameter(typeof(T), DIContainer, resolver => value(resolver)));

            return registration;
        }

        public static Registration WithParameter<T>(this Registration registration, Func<T> value)
        {
            var type = typeof(T);
            registration.CustomParameters.Add(type, new FuncInjectParameter(type, DIContainer, _ => value()));

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

        public static object Instantiate
        (
            this IResolver                              resolver,
            Type                                        type,
            IReadOnlyDictionary<Type, IInjectParameter> parameters = null
        )
        {
            var constructorParams = resolver.ResolveParams<ConstructorInjector>(type, null, parameters);

            var instance = Activator.CreateInstance(type, constructorParams.Select(param => param.Value).ToArray());

            resolver.Inject(instance, parameters);

            return instance;
        }

        public static T InstantiatePrefab<T>
        (
            this IResolver                              resolver,
            Component                                   prefab,
            Transform                                   parent     = null,
            IReadOnlyDictionary<Type, IInjectParameter> parameters = null
        ) where T : MonoBehaviour
        {
            return resolver.InstantiatePrefab(typeof(T), prefab, parent, parameters) as T;
        }

        public static object InstantiatePrefab
        (
            this IResolver                              resolver,
            Type                                        type,
            Component                                   prefab,
            Transform                                   parent     = null,
            IReadOnlyDictionary<Type, IInjectParameter> parameters = null
        )
        {
            var gameObjectInstance = Object.Instantiate(prefab.gameObject, parent);

            var component = gameObjectInstance.GetComponent(type);
            resolver.Inject(component, parameters);

            return component;
        }

        public static void Inject
        (
            this IResolver                              resolver,
            object                                      instance,
            IReadOnlyDictionary<Type, IInjectParameter> parameters = null
        )
        {
            var type = instance.GetType();
            resolver.ResolveParams<MethodInjector>(type, instance, parameters);
            resolver.ResolveParams<FieldInjector>(type, instance, parameters);
            resolver.ResolveParams<PropertyInjector>(type, instance, parameters);
        }

        public static void InjectGameObject
        (
            this IResolver                              resolver,
            GameObject                                  gameObject,
            IReadOnlyDictionary<Type, IInjectParameter> parameters = null
        )
        {
            foreach (var monoBehaviour in gameObject.GetComponents<MonoBehaviour>())
            {
                resolver.Inject(monoBehaviour, parameters);
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