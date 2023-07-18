using System.Runtime.Serialization;

namespace Poplike.Common.Exceptions;

[Serializable]
public class ValidateOnSaveException : Exception
{
    public ValidateOnSaveException()
    {
    }

    public ValidateOnSaveException(string? message) : base(message)
    {
    }

    public ValidateOnSaveException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ValidateOnSaveException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
