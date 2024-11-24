namespace ReflexDI
{
    public class RegistrationInNewPrefabProvider : RegistrationProvider
    {
        public RegistrationInNewPrefabProvider(Registration registration) : base(registration)
        {
        }

        private new RegistrationInNewPrefab Registration => (RegistrationInNewPrefab)base.Registration;

        protected override object GetSingletonInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            var instance = resolver.InstantiatePrefab(this.Registration.Prefab, this.Registration.Parent);
            var component = instance.GetComponent(this.Registration.ImplementedType);

            if (!string.IsNullOrEmpty(this.Registration.GameObjectName))
                instance.name = this.Registration.GameObjectName;

            this.SingletonInstance = component;

            return this.SingletonInstance;
        }
    }
}