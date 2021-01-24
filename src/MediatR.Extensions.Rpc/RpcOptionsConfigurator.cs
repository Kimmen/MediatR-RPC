using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Mediatr.Rpc;

namespace MediatR.Rpc.AspNetCore.DependencyInjection
{
    public static class RpcOptionsConfigurator
    {
        public static RpcOptions ScanRequests(this RpcOptions options, params Assembly[] assemblies)
        {
            var definedTypes = assemblies.SelectMany(a => a.DefinedTypes);
            return ScanRequests(options, definedTypes);
        }

        public static RpcOptions ScanRequests(this RpcOptions options, IEnumerable<Type> types)
        {
            options.Requests = RequestTypeScanner.FindRequestTypes(types).ToList();
            return options;
        }

        public static RpcOptions UseRequestNameMatchingConvention(this RpcOptions options)
        {
            static string Clean(string value)
            {
                return value
                    .ToLowerInvariant()
                    .Replace("request", null);
            }

            options.MatchingConvention = d => Clean(d.Name);

            return options;
        }
    }
}