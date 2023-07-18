using System.Runtime.Serialization;

namespace Poplike.Application.Account.Commands.RequestPasswordReset
{
    [Serializable]
    public class PreexistingPasswordResetRequestException : Exception
    {
        public PreexistingPasswordResetRequestException()
        {
        }

        public PreexistingPasswordResetRequestException(string? message) : base(message)
        {
        }

        public PreexistingPasswordResetRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PreexistingPasswordResetRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
