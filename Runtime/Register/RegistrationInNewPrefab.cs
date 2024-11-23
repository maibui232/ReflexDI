namespace ReflexDI
{
    using System;
    using UnityEngine;

    public class RegistrationInNewPrefab : RegistrationMono
    {
        public RegistrationInNewPrefab(Type implementedType, Component prefab) : base(implementedType)
        {
            this.Prefab               = prefab;
            this.RegistrationProvider = new RegistrationInNewPrefabProvider(this);
        }

        internal Component Prefab { get; }
    }
}