namespace ReflexDI
{
    using System;
    using JetBrains.Annotations;

    public class FuncInjectParameter : IInjectParameter
    {
        public Type   ParameterType { get; }
        public object Value         => this.valueResolver.Invoke(this.resolver);

        private readonly Func<IResolver, object> valueResolver;
        private readonly IResolver               resolver;

        public FuncInjectParameter(Type parameterType, IResolver resolver, [NotNull] Func<IResolver, object> valueResolver)
        {
            this.ParameterType = parameterType;
            this.resolver      = resolver;
            this.valueResolver = valueResolver;
        }
    }
}