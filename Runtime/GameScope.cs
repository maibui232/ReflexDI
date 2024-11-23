namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IBuilder
    {
        void AddRegistration(Registration registration);
    }

    public class GameScope : MonoBehaviour, IBuilder
    {
#region Fields

        [SerializeField] private GameObject[]    injectableObjects;
        [SerializeField] private MonoInstaller[] monoInstallers;

#endregion

#region Private

        private void Awake()
        {
            ReflexDISetting.Instance.InitializeRootScopeIfNeed();
            ReflexDISetting.Instance.BuildScopeIfNeed(this);
        }

        private void OnDestroy()
        {
            this.DestroyCallback?.Invoke(this);
        }

#endregion

#region Internal

        internal bool IsBuild { get; private set; }

        internal HashSet<Registration> Registrations { get; } = new();

        internal event Action<GameScope> BuildCallback;
        internal event Action<GameScope> DestroyCallback;

#endregion

#region IBuilder Implementation

        public void AddRegistration(Registration registration)
        {
            if (this.Registrations.Contains(registration)) ThrowExceptionExtensions.HasRegistration(registration.ImplementedType);

            this.Registrations.Add(registration);
        }

        public void Build(IResolver resolver)
        {
            this.InternalBuild(resolver);
            this.ResolveObjects(resolver);
        }

        private void InternalBuild(IResolver resolver)
        {
            foreach (var installer in this.monoInstallers) installer.Installing(this, resolver, this.transform);

            ReflexDIExtensions.DIContainer.Build(this);
            this.BuildCallback?.Invoke(this);
            this.IsBuild = true;
        }

        private void ResolveObjects(IResolver resolver)
        {
            foreach (var obj in this.injectableObjects)
            {
                if (obj == null) Debug.LogError($"Object {obj} is null");

                resolver.InjectGameObject(obj);
            }
        }

#endregion
    }
}