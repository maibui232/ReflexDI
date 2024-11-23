namespace ReflexDI
{
    using System;
    using UnityEngine;

    public class RegistrationOnNewPrefab : RegistrationMono
    {
        public RegistrationOnNewPrefab(Type implementedType, Component prefab) : base(implementedType)
        {
            this.Prefab               = prefab;
            this.RegistrationProvider = new RegistrationOnNewPrefabProvider(this);
        }

        internal Component Prefab { get; }
    }
}