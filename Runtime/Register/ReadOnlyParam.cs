namespace ReflexDI
{
    using System;

    public class ReadOnlyParam
    {
        public Type   Type  { get; }
        public object Value { get; }

        public ReadOnlyParam(Type type, object value)
        {
            this.Type  = type;
            this.Value = value;
        }
    }
}