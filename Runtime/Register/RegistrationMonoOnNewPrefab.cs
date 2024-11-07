namespace ReflexDI
{
    using System;
    using UnityEngine;

    public class RegistrationMonoOnNewPrefab : RegistrationMono
    {
        internal Component Prefab { get; }

        public RegistrationMonoOnNewPrefab(Type implementedType, Component prefab) : base(implementedType)
        {
            this.Prefab               = prefab;
            this.RegistrationProvider = new RegistrationMonoOnNewPrefabProvider(this);
        }
    }
}