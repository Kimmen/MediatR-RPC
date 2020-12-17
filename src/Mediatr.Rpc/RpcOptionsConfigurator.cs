using System.Linq;
using System.Reflection;

using Mediatr.Rpc;

namespace MediatR.Rpc.AspNetCore.DependencyInjection
{
    public static class RpcOptionsConfigurator
    {
        public static RpcOptions ScanRequests(this RpcOptions options, params Assembly[] assemblies)
        {
            var types = ReflectionTypeScanner.FindRequestTypes(assemblies).ToList();
            options.Requests = types;
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