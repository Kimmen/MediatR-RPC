using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    public class RpcEndpointOptions
    {
#nullable disable
        public string Path { get; set; } = "rpc";
        public string RequestNameRouteKey { get; set; } = "request";
        public string ContentType { get; set; }
        public Func<Type, HttpRequest, CancellationToken, Task<object>> DeserializeRequest { get; set; }
        public Func<object, string> SerializeResponse { get; set; }
        public Func<string, HttpContext, CancellationToken, Task> UnmatchedRequest { get; set; }
        public Func<string, HttpContext, CancellationToken, Task> HandlResponse { get; set; }

#nullable restore
    }
}