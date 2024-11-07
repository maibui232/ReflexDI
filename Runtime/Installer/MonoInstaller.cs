namespace ReflexDI
{
    using UnityEngine;

    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void Installing(IBuilder builder, IResolver resolver, Transform parent);
    }
}