using System.Net.Mime;
using System.Threading.Tasks;
using Mediatr.Rpc;
using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    public class MediatrRpcMiddleware
    {
        private readonly RequestDelegate next;
        private readonly RpcEndpointOptions options;
        private readonly RpcCallExecuter rpcCaller;

        public MediatrRpcMiddleware(RequestDelegate next, RpcEndpointOptions options, RpcCallExecuter rpcCaller)
        {
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
            var requestToken = context.Request.RouteValues[options.RequestNameRouteKey]?.ToString();

            if (string.IsNullOrWhiteSpace(requestToken))
            {
                await next(context); 
            }

            var bodyStream = context.Request.Body;
            
            var result = await rpcCaller.Process(requestToken, (t, ct) => options.DeserializeRequest(t, context.Request, ct), cancellationToken);

            if (result.FoundHandler)
            {
                var serializedResult = options.SerializeResponse(result.Response);
                await options.HandlResponse(serializedResult, context, cancellationToken);
            }
            else
            {
                await next(context); 
            }
        }
    }
}