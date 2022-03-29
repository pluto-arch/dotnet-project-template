using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    [DebuggerStepThrough]
    public static class ValueCheck
    {
        public static T NotNull<T>(T value, [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        public static T NotNull<T>(T value, [NotNull] string parameterName, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }

            return value;
        }
    }
}