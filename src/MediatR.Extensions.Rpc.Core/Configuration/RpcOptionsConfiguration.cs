using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.Rpc
{
    /// <summary>
    /// Configurations methods for RPC options.
    /// </summary>
    public static class RpcOptionsConfiguration
    {
        /// <summary>
        /// Scans for request types in the given assemblies.
        /// </summary>
        /// <param name="options">Options to register the found requests in.</param>
        /// <param name="assemblies">Assemblies for scanning request types.</param>
        /// <returns>The updated options.</returns>
        public static RpcOptions ScanRequests(this RpcOptions options, params Assembly[] assemblies)
        {
            var definedTypes = assemblies.SelectMany(a => a.DefinedTypes);
            return ScanRequests(options, definedTypes);
        }

        /// <summary>
        /// Scans for request types in the given collection of types.
        /// </summary>
        /// <param name="options">Options to register the found requests in.</param>
        /// <param name="types">A list for scanning request types.</param>
        /// <returns>The updated options.</returns>
        public static RpcOptions ScanRequests(this RpcOptions options, IEnumerable<Type> types)
        {
            options.Requests = RequestTypeScanner.FindRequestTypes(types).ToList();
            return options;
        }

        /// <summary>
        /// Use the requests name when matching types.
        /// </summary>
        /// <param name="options">Options to set the convention for.</param>
        /// <returns>The updated options.</returns>
        public static RpcOptions UseExactRequestTypeNameMatchingConvention(this RpcOptions options)
        {
            options.MatchingConvention = d => d.Name;

            return options;
        }
    }
}