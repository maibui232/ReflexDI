namespace ReflexDI
{
    using System;

    public class RegistrationInstance : Registration
    {
        public RegistrationInstance(Type implementedType, object instance) : base(implementedType)
        {
            this.CachedInstance       = instance;
            this.RegistrationProvider = new RegistrationInstanceProvider(this);
        }

        internal object CachedInstance { get; }
    }
}