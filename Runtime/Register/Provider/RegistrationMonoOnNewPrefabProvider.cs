namespace ReflexDI
{
    using UnityEngine;

    public class RegistrationMonoOnNewPrefabProvider : RegistrationProvider
    {
        private new RegistrationMonoOnNewPrefab Registration => (RegistrationMonoOnNewPrefab)base.Registration;

        public RegistrationMonoOnNewPrefabProvider(Registration registration) : base(registration)
        {
        }

        public override object SpawnInstance(IResolver resolver)
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