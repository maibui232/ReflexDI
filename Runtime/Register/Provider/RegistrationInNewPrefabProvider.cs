namespace ReflexDI
{
    using UnityEngine;

    public class RegistrationInNewPrefabProvider : RegistrationProvider
    {
        private new RegistrationInNewPrefab Registration => (RegistrationInNewPrefab)base.Registration;

        public RegistrationInNewPrefabProvider(Registration registration) : base(registration)
        {
        }

        public override object SpawnInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            var instance = resolver.InstantiatePrefab(this.Registration.ImplementedType, this.Registration.Prefab, this.Registration.Parent);

            if (!string.IsNullOrEmpty(this.Registration.GameObjectName))
            {
                if (instance is Component component)
                {
                    component.gameObject.name = this.Registration.GameObjectName;
                }
            }

            this.SingletonInstance = instance;

            return this.SingletonInstance;
        }
    }
}