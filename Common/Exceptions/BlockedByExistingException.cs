using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class BlockedByExistingException : Exception
    {
        public BlockedByExistingException()
        {
        }

        public BlockedByExistingException(string? message) : base(message)
        {
        }

        public BlockedByExistingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BlockedByExistingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
