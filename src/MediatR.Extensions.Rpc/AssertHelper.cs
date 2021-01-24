using System;

namespace Mediatr.Rpc
{
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