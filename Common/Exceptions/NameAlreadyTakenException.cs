using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class NameAlreadyTakenException : Exception
    {
        public NameAlreadyTakenException()
        {
        }

        public NameAlreadyTakenException(string? message) : base(message)
        {
        }

        public NameAlreadyTakenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NameAlreadyTakenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
