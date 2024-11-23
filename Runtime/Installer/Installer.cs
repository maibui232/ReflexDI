namespace ReflexDI
{
    using System;
    using UnityEngine;

    public abstract class Installer<TDerived> : IInstaller where TDerived : class, IInstaller
    {
        public abstract void Installing(IBuilder builder, IResolver resolver, Transform parent);

        public static void Install(IBuilder builder, IResolver resolver, Transform parent)
        {
            Activator.CreateInstance<TDerived>().Installing(builder, resolver, parent);
        }
    }

    public abstract class Installer<TDerived, TConcrete> : IInstaller where TDerived : class, IInstaller
    {
        public abstract void Installing(IBuilder builder, IResolver resolver, Transform parent);

        public static void Install(IBuilder builder, IResolver resolver, Transform parent, TConcrete concrete)
        {
            var ctor = typeof(TDerived).GetConstructors();
            if (ctor.Length == 0)
            {
                CreateInstallerIfNotExists(builder, resolver, parent);

                return;
            }

            if (ctor.Length != 1) throw new ArgumentException($"Type '{typeof(TDerived).FullName}' does not have a constructor with 1 parameter");

            var ctorParams = ctor[0].GetParameters();
            if (ctorParams.Length == 0)
            {
                CreateInstallerIfNotExists(builder, resolver, parent);

                return;
            }

            if (ctorParams.Length != 1) throw new ArgumentException($"Type '{typeof(TDerived).FullName}' does not have a constructor with 1 parameter");

            var installer = Activator.CreateInstance(typeof(TDerived), concrete) as TDerived;
            installer?.Installing(builder, resolver, parent);
        }

        private static void CreateInstallerIfNotExists(IBuilder builder, IResolver resolver, Transform parent)
        {
            var installer = Activator.CreateInstance<TDerived>();
            installer.Installing(builder, resolver, parent);
        }
    }
}