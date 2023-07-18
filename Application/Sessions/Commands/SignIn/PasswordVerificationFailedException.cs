using System.Runtime.Serialization;

namespace Poplike.Application.Sessions.Commands.SignIn
{
    public class PasswordVerificationFailedException : Exception
    {
        public PasswordVerificationFailedException()
        {
        }

        public PasswordVerificationFailedException(string? message) : base(message)
        {
        }

        public PasswordVerificationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PasswordVerificationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
