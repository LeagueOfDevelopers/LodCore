using System;
using System.Runtime.Serialization;

namespace LodCoreLibrary.Domain.Exceptions
{
    [Serializable]
    public class AccountAlreadyExistsException : Exception
    {
        public AccountAlreadyExistsException()
        {
        }

        public AccountAlreadyExistsException(string message) : base(message)
        {
        }

        public AccountAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AccountAlreadyExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}