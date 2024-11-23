namespace ReflexDI
{
    using UnityEngine;

    public class RegistrationOnNewPrefabProvider : RegistrationProvider
    {
        public RegistrationOnNewPrefabProvider(Registration registration) : base(registration)
        {
        }

        private new RegistrationOnNewPrefab Registration => (RegistrationOnNewPrefab)base.Registration;

        protected override object GetSingletonInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            var gameObjectInstance = Object.Instantiate(this.Registration.Prefab.gameObject, this.Registration.Parent);
            var component          = gameObjectInstance.AddComponent(this.Registration.ImplementedType);

            resolver.Inject(component, this.Registration.CustomParameters);
            this.SingletonInstance = component;

            return this.SingletonInstance;
        }
    }
}