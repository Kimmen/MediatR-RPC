
using Mediatr.Rpc;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Extensions.Rpc.Functions
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class RpcHttpFunctionOptions
    {
        public Func<(Type TargetRequestType, HttpRequest HttpRequest), CancellationToken, Task<object>> DeserializeRequest { get; set; }

        public Func<(IRpcResult Result, HttpRequest HttpRequest), CancellationToken, Task<IActionResult>> HandleResponse { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}


