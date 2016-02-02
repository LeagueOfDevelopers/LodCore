using System;
using System.Runtime.Serialization;

namespace UserManagement.Domain
{
    [Serializable]
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException()
        {
        }

        public TokenNotFoundException(string message) : base(message)
        {
        }

        public TokenNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TokenNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}