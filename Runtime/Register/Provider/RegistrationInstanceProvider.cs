namespace ReflexDI
{
    public class RegistrationInstanceProvider : RegistrationProvider
    {
        public RegistrationInstanceProvider(Registration registration) : base(registration)
        {
        }

        private new RegistrationInstance Registration => (RegistrationInstance)base.Registration;

        protected override object GetSingletonInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            resolver.Inject(this.Registration.CachedInstance, this.Registration.CustomParameters);
            this.SingletonInstance = this.Registration.CachedInstance;

            return this.SingletonInstance;
        }
    }
}