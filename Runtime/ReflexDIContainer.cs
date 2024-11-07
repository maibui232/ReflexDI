namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ReflexDIContainer : IResolver
    {
        private readonly Dictionary<Type, Registration> typeToRegistration = new();

        public void Build(GameScope gameScope)
        {
            gameScope.DestroyCallback += this.OnDestroyScope;
            foreach (var registration in gameScope.Registrations)
            {
                if (registration.RegistrationProvider.IsNonLazy)
                {
                    registration.RegistrationProvider.SpawnInstance(this);
                }

                foreach (var type in registration.RegisterTypes.Where(type => !this.typeToRegistration.TryAdd(type, registration)))
                {
                    ThrowExceptionExtensions.HasRegistration(type);
                }
            }
        }

        private void OnDestroyScope(GameScope scope)
        {
            scope.DestroyCallback -= this.OnDestroyScope;
            foreach (var type in scope.Registrations.SelectMany(registration => registration.RegisterTypes))
            {
                this.typeToRegistration.Remove(type, out _);
            }
        }

        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (!this.typeToRegistration.TryGetValue(type, out var registration))
            {
                ThrowExceptionExtensions.HasNotRegistration(type);

                return default;
            }

            return registration.RegistrationProvider.SpawnInstance(this);
        }
    }
}