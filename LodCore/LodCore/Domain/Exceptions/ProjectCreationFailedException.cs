using System;
using System.Runtime.Serialization;

namespace LodCore.Domain.Exceptions
{
    [Serializable]
    public class ProjectCreationFailedException : Exception
    {
        public ProjectCreationFailedException()
        {
        }

        public ProjectCreationFailedException(string message) : base(message)
        {
        }

        public ProjectCreationFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProjectCreationFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}