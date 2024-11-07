namespace ReflexDI
{
    public class RegistrationProvider
    {
        public RegistrationProvider(Registration registration)
        {
            this.Registration = registration;
        }

        internal Registration Registration { get; }
        internal bool         IsNonLazy    { get; set; }

        public virtual object SpawnInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            this.SingletonInstance = resolver.Instantiate(this.Registration.ImplementedType);

            return this.SingletonInstance;
        }

        protected object SingletonInstance { get; set; }
    }
}