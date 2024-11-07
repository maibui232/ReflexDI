namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    public class Registration
    {
        public   Type                   ImplementedType      { get; }
        public   HashSet<Type>          RegisterTypes        { get; } = new();
        internal HashSet<Type>          DependencyTypes      { get; } = new();
        internal HashSet<ReadOnlyParam> CustomParams         { get; } = new();
        internal RegistrationProvider   RegistrationProvider { get; set; }

        public Registration(Type implementedType)
        {
            this.ImplementedType      = implementedType;
            this.RegistrationProvider = new RegistrationProvider(this);
        }
    }
}