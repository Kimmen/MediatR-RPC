using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    public class RpcEndpointOptions
    {
#nullable disable
        /// <summary>
        /// The root path for th RPC endpoint.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Deserializer for the requests.
        /// </summary>
        public Func<Type, HttpContext, CancellationToken, Task<object>> DeserializeRequest { get; set; }
        /// <summary>
        /// Serializer for the responses.
        /// </summary>
        public Func<object, HttpContext, CancellationToken, Task> SerializeResponse { get; set; }
        /// <summary>
        /// Handler for responses.
        /// </summary>
        public Func<IRpcResult, HttpContext, CancellationToken, Task> HandlResponse { get; set; }

#nullable restore
    }
}