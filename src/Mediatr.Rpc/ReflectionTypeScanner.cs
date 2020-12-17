using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;

namespace Mediatr.Rpc
{
    public class ReflectionTypeScanner
    {
        private static readonly Type MediatrRequestType = typeof(IRequest<>);
        public static IEnumerable<Type> FindRequestTypes(params Assembly[] assemblies)
        {
            var types = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !IsOpenGeneric(t))
                .Where(IsMediatrRequest);

            return types;
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