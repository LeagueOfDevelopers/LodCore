using System;
using System.Runtime.Serialization;

namespace ProjectManagement.Domain
{
    [Serializable]
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException()
        {
        }

        public ProjectNotFoundException(string message) : base(message)
        {
        }

        public ProjectNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProjectNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}