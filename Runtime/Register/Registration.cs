namespace ReflexDI
{
    using System;
    using System.Collections.Generic;

    public class Registration
    {
        public Registration(Type implementedType)
        {
            this.ImplementedType      = implementedType;
            this.RegistrationProvider = new RegistrationProvider(this);
        }

        internal Type                               ImplementedType      { get; }
        internal HashSet<Type>                      RegisterTypes        { get; } = new();
        internal HashSet<Type>                      EntryPointTypes      { get; } = new();
        internal HashSet<Type>                      DependencyTypes      { get; } = new();
        internal Dictionary<Type, IInjectParameter> CustomParameters     { get; } = new();
        internal RegistrationProvider               RegistrationProvider { get; set; }
    }
}