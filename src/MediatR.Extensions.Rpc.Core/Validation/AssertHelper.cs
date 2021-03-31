using System;

namespace MediatR.Rpc.Validation
{
    /// <summary>
    /// Helper methods for validating for consistent messages.
    /// </summary>
    internal static class AssertHelper
    {
        public static void ValidateIsNotNull(object value, string propName)
        {
            if(value == null)
            {
                throw new ArgumentNullException(propName, $"Need to specify {propName}, cannot be null");
            }
        }
    }
}