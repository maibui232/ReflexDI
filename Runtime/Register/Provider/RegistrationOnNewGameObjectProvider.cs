namespace ReflexDI
{
    using UnityEngine;

    public class RegistrationOnNewGameObjectProvider : RegistrationProvider
    {
        public RegistrationOnNewGameObjectProvider(Registration registration) : base(registration)
        {
        }

        private new RegistrationMono Registration => (RegistrationOnNewGameObject)base.Registration;

        protected override object GetSingletonInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            var instance = new GameObject();
            instance.transform.SetParent(this.Registration.Parent);
            var component = instance.AddComponent(this.Registration.ImplementedType);

            resolver.Inject(component, this.Registration.CustomParameters);
            this.SingletonInstance = component;

            return this.SingletonInstance;
        }
    }
}