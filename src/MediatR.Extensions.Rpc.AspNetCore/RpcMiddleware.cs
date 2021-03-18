using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    /// <summary>
    /// Middleware for handling the HTTP routing.
    /// </summary>
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

            await (false == foundRequestNameRouteKey
                ? HandleRouteKeyNotFound(context, cancellationToken)
                : HandleRouteKeyFound(requestNameValue?.ToString() ?? string.Empty, context, cancellationToken));
        }

        private async Task HandleRouteKeyNotFound(HttpContext context, System.Threading.CancellationToken cancellationToken)
        {
            await options.HandlResponse(new NotFoundRequestResult
            {
                RequestName = string.Empty
            }, context, cancellationToken);
        }

        private async Task HandleRouteKeyFound(string requestName, HttpContext context, System.Threading.CancellationToken cancellationToken)
        {
            var result = await rpcCaller.Process(requestName, (t, ct) => options.DeserializeRequest(t, context, ct), cancellationToken);

            await options.HandlResponse(result, context, cancellationToken);
        }
    }
}