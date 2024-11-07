namespace ReflexDI
{
    using System;

    public class RegistrationMonoOnNewGameObject : RegistrationMono
    {
        public RegistrationMonoOnNewGameObject(Type implementedType) : base(implementedType)
        {
            this.RegistrationProvider = new RegistrationMonoOnNewGameObjectProvider(this);
        }
    }
}