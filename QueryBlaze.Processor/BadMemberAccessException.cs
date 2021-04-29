using System;

namespace QueryBlaze.Processor
{
    [Serializable]
    public class BadMemberAccessException : Exception
    {
        public BadMemberAccessException() { }

        public BadMemberAccessException(string message) : base(message) { }

        public BadMemberAccessException(string message, Exception inner) : base(message, inner) { }

        protected BadMemberAccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
