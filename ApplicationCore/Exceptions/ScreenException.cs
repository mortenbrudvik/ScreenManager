using System;

namespace ApplicationCore.Exceptions
{
    public class ScreenException : Exception
    {
        protected ScreenException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public ScreenException(string message) : base(message) { }
        public ScreenException(string message, Exception innerException) : base(message, innerException) { }
    }
}