namespace ReflexDI
{
    public class RegistrationInstanceProvider : RegistrationProvider
    {
        private new RegistrationInstance Registration => (RegistrationInstance)base.Registration;

        public RegistrationInstanceProvider(Registration registration) : base(registration)
        {
        }

        public override object SpawnInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            resolver.Inject(this.Registration.CachedInstance);
            this.SingletonInstance = this.Registration.CachedInstance;

            return this.SingletonInstance;
        }
    }
}