namespace ReflexDI
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class InjectableAttribute : Attribute
    {
    }
}