using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    /// <summary>
    /// Options for the RPC endpoint.
    /// </summary>
    public class RpcEndpointOptions
    {
#nullable disable
        /// <summary>
        /// The root path for th RPC endpoint.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Deserializes Http data to the target type.
        /// </summary>
        public Func<(Type TargetRequestType, HttpContext HttpContext), CancellationToken, Task<object>> DeserializeRequest { get; set; }
        /// <summary>
        /// Serializer response to the Http stream.
        /// </summary>
        public Func<(IRpcResult Result, HttpContext HttpContext), CancellationToken, Task> SerializeResponse { get; set; }
#nullable restore
    }
}