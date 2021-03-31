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
        private readonly IRpcRequestRunner rpcCaller;

        public RpcMiddleware(RequestDelegate next, RpcEndpointOptions options, IRpcRequestRunner rpcCaller)
        {
            RpcMiddlewareValidator.ValidateOptions(options);
            RpcMiddlewareValidator.ValidateCaller(rpcCaller);

            this.rpcCaller = rpcCaller;
            this.options = options;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var cancellationToken = context.RequestAborted;
            if(context.Request.RouteValues.TryGetValue(Known.RouteValues.RequestName, out var requestNameRouteValue))
            {
                await HandleRouteKeyFound(requestNameRouteValue, context, cancellationToken);
            }
            else
            {
                await HandleRouteKeyNotFound(context, cancellationToken);
            }
        }

        private async Task HandleRouteKeyNotFound(HttpContext context, System.Threading.CancellationToken cancellationToken)
        {
            var response = (new RequestNameRouteValueNotFoundResult(), context);
            await options.SerializeResponse(response, cancellationToken);
        }

        private async Task HandleRouteKeyFound(object? routeValue, HttpContext context, System.Threading.CancellationToken cancellationToken)
        {
            var requestName = routeValue?.ToString() ?? string.Empty;
            var result = await rpcCaller.Process(requestName, (t, ct) => options.DeserializeRequest((t, context), ct), cancellationToken);

            var response = (result, context);
            await options.SerializeResponse(response, cancellationToken);
        }
    }
}