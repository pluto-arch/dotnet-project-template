using System;

namespace PlutoNetCoreTemplate.Infrastructure.Exceptions
{
    public class RepeatedCommandException : Exception
    {
        public RepeatedCommandException()
        { }

        public RepeatedCommandException(string message)
            : base(message)
        { }

        public RepeatedCommandException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}