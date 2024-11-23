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

#region Destruction Instance

        internal void DestructInstance()
        {
            if (this.SingletonInstance == null) return;
            this.Registration.UnRegisterGameLoopEvent(this.SingletonInstance);
            this.Registration.DisposeObject(this.SingletonInstance);
        }

#endregion

#region Construction Instance

        internal object ConstructInstance(IResolver resolver)
        {
            var instance = this.GetSingletonInstance(resolver);
            this.Registration.InitializeObject(instance);
            this.Registration.RegisterGameLoopEvent(instance);

            return instance;
        }

        protected virtual object GetSingletonInstance(IResolver resolver)
        {
            if (this.SingletonInstance != null) return this.SingletonInstance;

            this.SingletonInstance = resolver.Instantiate(this.Registration.ImplementedType, this.Registration.CustomParameters);

            return this.SingletonInstance;
        }

        protected object SingletonInstance { get; set; }

#endregion
    }
}