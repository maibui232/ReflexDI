namespace ReflexDI
{
    using System;

    public class RegistrationInstance : Registration
    {
        internal object CachedInstance { get; }

        public RegistrationInstance(Type implementedType, object instance) : base(implementedType)
        {
            this.CachedInstance       = instance;
            this.RegistrationProvider = new RegistrationInstanceProvider(this);
        }
    }
}