using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class NotPermittedException : Exception
    {
        public NotPermittedException()
        {
        }

        public NotPermittedException(string? message) : base(message)
        {
        }

        public NotPermittedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotPermittedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
