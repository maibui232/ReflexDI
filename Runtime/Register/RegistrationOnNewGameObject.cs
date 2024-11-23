namespace ReflexDI
{
    using System;

    public class RegistrationOnNewGameObject : RegistrationMono
    {
        public RegistrationOnNewGameObject(Type implementedType) : base(implementedType)
        {
            this.RegistrationProvider = new RegistrationOnNewGameObjectProvider(this);
        }
    }
}