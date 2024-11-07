namespace ReflexDI
{
    using System;
    using UnityEngine;

    public abstract class RegistrationMono : Registration
    {
        protected RegistrationMono(Type implementedType) : base(implementedType)
        {
        }

        internal string    GameObjectName { get; set; }
        internal Transform Parent         { get; set; }
    }
}