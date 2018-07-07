using System;
using System.Runtime.Serialization;

namespace LodCoreLibrary.Domain.Exceptions
{
    [Serializable]
    public class ConfirmationFailedException : Exception
    {
        public ConfirmationFailedException()
        {
        }

        public ConfirmationFailedException(string message) : base(message)
        {
        }

        public ConfirmationFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConfirmationFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}