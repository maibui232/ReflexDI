namespace ReflexDI
{
    using System;

    public class TypeInjectParameter : IInjectParameter
    {
        public Type   ParameterType { get; }
        public object Value         { get; }

        public TypeInjectParameter(Type parameterType, object value)
        {
            this.ParameterType = parameterType;
            this.Value         = value;
        }
    }
}