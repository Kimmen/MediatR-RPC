using System;
using System.Collections.Generic;
using System.Linq;

using MediatR;

namespace Mediatr.Rpc
{
    public class RequestTypeScanner
    {
        private static readonly Type MediatrRequestType = typeof(IRequest<>);

        public static IEnumerable<Type> FindRequestTypes(IEnumerable<Type> types)
        {
            var requestTypes = types
                .Where(t => !IsOpenGeneric(t))
                .Where(t => !t.IsInterface)
                .Where(IsMediatrRequest);

            return requestTypes;
        }

        public static bool IsMediatrRequest(Type type)
        {
            return type.GetInterfaces()
                .Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == MediatrRequestType)
                .Any();
        }
        public static bool IsOpenGeneric(Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }
    }
}