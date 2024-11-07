namespace ReflexDI
{
    using UnityEngine;

    public interface IInstaller
    {
        void Installing(IBuilder builder, IResolver resolver, Transform parent);
    }
}