namespace ReflexDI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IBuilder
    {
        void RegisterBuildCallback(Action<IResolver>  callback);
        void RegisterDisposeCallback(Action<IBuilder> callback);
        void AddRegistration(Registration             registration);
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

        internal event Action<IResolver> BuildCallback;
        internal event Action<GameScope> DestroyCallback;

#endregion

#region IBuilder Implementation

        void IBuilder.RegisterBuildCallback(Action<IResolver> callback)
        {
            this.BuildCallback += callback;
        }

        void IBuilder.RegisterDisposeCallback(Action<IBuilder> callback)
        {
            this.DestroyCallback += callback;
        }

        void IBuilder.AddRegistration(Registration registration)
        {
            if (!this.Registrations.Add(registration))
            {
                throw new RegistrationExistException(registration.ImplementedType);
            }
        }

#endregion

        internal void Build(IResolver resolver)
        {
            foreach (var installer in this.monoInstallers)
            {
                installer.Installing(this, resolver, this.transform);
            }

            ReflexDIExtensions.DIContainer.Build(this);
            this.BuildCallback?.Invoke(ReflexDIExtensions.DIContainer);
            this.IsBuild = true;

            foreach (var obj in this.injectableObjects)
            {
                if (obj == null) Debug.LogError($"Object {obj} is null");

                resolver.InjectGameObject(obj);
            }
        }
    }
}