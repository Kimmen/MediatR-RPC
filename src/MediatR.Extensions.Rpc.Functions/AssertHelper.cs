
using System;

namespace MediatR.Rpc.Azure.Functions
{
    internal static class AssertHelper
    {
        internal static void ValidateIsNotNull(object o, string propName)
        {
            if (o == null)
            {
                throw new ArgumentNullException(propName, $"Need to specify {propName}, cannot be null");
            }
        }

        internal static void ValidateIsNotEmpty(string value, string propName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Need to specify {propName}, cannot be empty.", propName);
            }
        }
    }
}