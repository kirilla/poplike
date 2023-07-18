using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions
{
    [Serializable]
    public class FeatureTurnedOffException : Exception
    {
        public FeatureTurnedOffException()
        {
        }

        public FeatureTurnedOffException(string? message) : base(message)
        {
        }

        public FeatureTurnedOffException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected FeatureTurnedOffException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
