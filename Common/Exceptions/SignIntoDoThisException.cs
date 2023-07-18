using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class SignIntoDoThisException : Exception
    {
        public SignIntoDoThisException()
        {
        }

        public SignIntoDoThisException(string? message) : base(message)
        {
        }

        public SignIntoDoThisException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SignIntoDoThisException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
