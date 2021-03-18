

using MediatR.Rpc;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Azure.Functions
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <summary>
    /// Options for the RCP Http triggers.
    /// </summary>
    public class RpcHttpFunctionOptions
    {
        /// <summary>
        /// Deserializer for the requests.
        /// </summary>
        public Func<(Type TargetRequestType, HttpRequest HttpRequest), CancellationToken, Task<object>> DeserializeRequest { get; set; }
        /// <summary>
        /// Handler for responses.
        /// </summary>
        public Func<(IRpcResult Result, HttpRequest HttpRequest), CancellationToken, Task<IActionResult>> HandleResponse { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}


