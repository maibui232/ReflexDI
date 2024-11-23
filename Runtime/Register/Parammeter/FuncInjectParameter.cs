namespace ReflexDI
{
    using System;
    using JetBrains.Annotations;

    public class FuncInjectParameter : IInjectParameter
    {
        private readonly IResolver resolver;

        private readonly Func<IResolver, object> valueResolver;

        public FuncInjectParameter(Type parameterType, IResolver resolver, [NotNull] Func<IResolver, object> valueResolver)
        {
            this.ParameterType = parameterType;
            this.resolver      = resolver;
            this.valueResolver = valueResolver;
        }

        public Type   ParameterType { get; }
        public object Value         => this.valueResolver.Invoke(this.resolver);
    }
}