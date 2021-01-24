using System.Threading.Tasks;

using Mediatr.Rpc;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    internal class RpcMiddleware
    {
#pragma warning disable IDE0052 //Needed for the class to be created as a Middlware. Keep it around for later use.
        private readonly RequestDelegate next;
#pragma warning restore IDE0052 

        private readonly RpcEndpointOptions options;
        private readonly RpcCaller rpcCaller;

        public RpcMiddleware(RequestDelegate next, RpcEndpointOptions options, RpcCaller rpcCaller)
        {
            RpcMiddlewareValidator.ValidateOptions(options);
            RpcMiddlewareValidator.ValidateCaller(rpcCaller);

            this.rpcCaller = rpcCaller;
            this.options = options;
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            return context.Request.Method.ToLowerInvariant() switch
            {
                "post" => ProcessRequest(context),
                _ => ResponeWithMethodNotAllowed(context)
            };
        }

        private Task ResponeWithMethodNotAllowed(HttpContext context)
        {
            var response = context.Response;
            response.Headers.Add("Allow", $"{HttpMethods.Post}");
            response.StatusCode = StatusCodes.Status405MethodNotAllowed;

            return Task.CompletedTask;
        }

        private async Task ProcessRequest(HttpContext context)
        {
            var cancellationToken = context.RequestAborted;
            var foundRequestNameRouteKey = context.Request.RouteValues.TryGetValue(Known.RouteValues.RequestName, out var requestNameValue);

            if (false == foundRequestNameRouteKey)
            {
                await options.UnmatchedRequest(string.Empty, context, cancellationToken);
                return;
            }

            var requestName = requestNameValue.ToString() ?? string.Empty;
            var result = await rpcCaller.Process(requestName, (t, ct) => options.DeserializeRequest(t, context, ct), cancellationToken);

            if (result.FoundHandler)
            {
                await options.HandlResponse(result.Response, context, cancellationToken);
            }
            else
            {
                await options.UnmatchedRequest(requestName, context, cancellationToken);
            }
        }
    }
}