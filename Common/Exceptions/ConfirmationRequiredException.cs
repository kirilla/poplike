using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class ConfirmationRequiredException : Exception
    {
        public ConfirmationRequiredException()
        {
        }

        public ConfirmationRequiredException(string? message) : base(message)
        {
        }

        public ConfirmationRequiredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ConfirmationRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
