namespace ReflexDI
{
    using System;
    using UnityEngine;

    public class RegistrationInNewPrefab : RegistrationMono
    {
        internal Component Prefab { get; }

        public RegistrationInNewPrefab(Type implementedType, Component prefab) : base(implementedType)
        {
            this.Prefab               = prefab;
            this.RegistrationProvider = new RegistrationInNewPrefabProvider(this);
        }
    }
}