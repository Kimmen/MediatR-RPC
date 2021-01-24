using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    public class RpcEndpointOptions
    {
#nullable disable
        public string Path { get; set; }
        public Func<Type, HttpContext, CancellationToken, Task<object>> DeserializeRequest { get; set; }
        public Func<object, HttpContext, CancellationToken, Task<string>> SerializeResponse { get; set; }
        public Func<string, HttpContext, CancellationToken, Task> UnmatchedRequest { get; set; }
        public Func<object, HttpContext, CancellationToken, Task> HandlResponse { get; set; }

#nullable restore
    }
}