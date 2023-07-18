using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException()
        {
        }

        public ValidationErrorException(string? message) : base(message)
        {
        }

        public ValidationErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ValidationErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
