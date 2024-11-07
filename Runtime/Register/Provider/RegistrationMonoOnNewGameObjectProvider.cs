namespace ReflexDI
{
    using UnityEngine;

    public class RegistrationMonoOnNewGameObjectProvider : RegistrationProvider
    {
        private new RegistrationMono Registration => (RegistrationMonoOnNewGameObject)base.Registration;

        public RegistrationMonoOnNewGameObjectProvider(Registration registration) : base(registration)
        {
        }

        public override object SpawnInstance(IResolver resolver)
        {
            return base.SpawnInstance(resolver);
        }
    }
}