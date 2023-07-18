using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class PhoneNumberAlreadyTakenException : Exception
    {
        public PhoneNumberAlreadyTakenException()
        {
        }

        public PhoneNumberAlreadyTakenException(string? message) : base(message)
        {
        }

        public PhoneNumberAlreadyTakenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PhoneNumberAlreadyTakenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
