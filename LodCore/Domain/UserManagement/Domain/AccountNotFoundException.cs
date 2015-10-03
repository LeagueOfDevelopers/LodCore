using System;
using System.Runtime.Serialization;

namespace UserManagement.Domain
{
    [Serializable]
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string message) : base(message)
        {
        }

        public AccountNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AccountNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}