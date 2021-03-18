using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.Rpc
{
    /// <summary>
    /// Scans for MediatR types.
    /// </summary>
    public class RequestTypeScanner
    {
        private static readonly Type MediatrRequestType = typeof(IRequest<>);

        /// <summary>
        /// Finds implementations of <see cref="IRequest{TResponse}"/> given a list of types.
        /// </summary>
        /// <param name="types">List of types to look for <see cref="IRequest{TResponse}"/>.</param>
        /// <returns>A list of types which are considered implementations of <see cref="IRequest{TResponse}"/>.</returns>
        public static IEnumerable<Type> FindRequestTypes(IEnumerable<Type> types)
        {
            var requestTypes = types
                .Where(t => !IsOpenGeneric(t))
                .Where(t => !t.IsInterface)
                .Where(IsMediatrRequest);

            return requestTypes;
        }

        /// <summary>
        /// Determines if the specified type is considered a <see cref="IRequest{TResponse}"/>.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns>True if considered <see cref="IRequest{TResponse}"/>; otherwise false.</returns>
        public static bool IsMediatrRequest(Type type)
        {
            return type.GetInterfaces()
                .Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == MediatrRequestType)
                .Any();
        }

        /// <summary>
        /// Determines of the specified type is considered to be open generic.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns>True if considered open generic; otherwise false.</returns>
        public static bool IsOpenGeneric(Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }
    }
}