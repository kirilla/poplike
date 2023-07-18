using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class WordPreventedException : Exception
    {
        public WordPreventedException()
        {
        }

        public WordPreventedException(string? message) : base(message)
        {
        }

        public WordPreventedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected WordPreventedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
