namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ReflexDIContainer : IResolver
    {
        private readonly Dictionary<Type, Registration> typeToRegistration = new();

        public ReflexDIContainer()
        {
            var thisType = typeof(IResolver);
            this.typeToRegistration.Add(thisType, new RegistrationInstance(thisType, this).AsSelf());
        }

#region IResolver implementation

        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (!this.typeToRegistration.TryGetValue(type, out var registration))
            {
                throw new RegistrationNotExistException(type);
            }

            return registration.RegistrationProvider.ConstructInstance(this);
        }

#endregion

        internal void Build(GameScope gameScope)
        {
            gameScope.DestroyCallback += this.OnDestroyScope;
            foreach (var registration in gameScope.Registrations)
            {
                if (registration.RegisterTypes.Count == 0)
                {
                    registration.AsSelf();
                }

                foreach (var type in registration.RegisterTypes.Where(type => !this.typeToRegistration.TryAdd(type, registration)))
                {
                    throw new RegistrationExistException(type);
                }

                if (registration.RegistrationProvider.IsNonLazy)
                {
                    registration.RegistrationProvider.ConstructInstance(this);
                }
            }
        }

        private void OnDestroyScope(GameScope scope)
        {
            scope.DestroyCallback -= this.OnDestroyScope;
            foreach (var (_, registration) in this.typeToRegistration)
            {
                registration.RegistrationProvider.DestructInstance();
            }

            this.typeToRegistration.Clear();
        }
    }
}