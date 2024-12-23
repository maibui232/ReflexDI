namespace ReflexDI
{
    using System;

    public class TypeInjectParameter : IInjectParameter
    {
        public TypeInjectParameter(Type parameterType, object value)
        {
            this.ParameterType = parameterType;
            this.Value         = value;
        }

        public Type   ParameterType { get; }
        public object Value         { get; }
    }
}