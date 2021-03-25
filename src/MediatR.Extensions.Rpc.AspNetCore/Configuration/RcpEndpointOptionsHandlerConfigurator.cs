using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore.Configuration
{
    public static class RcpEndpointOptionsHandlerConfigurator
    {
        /// <summary>
        /// If the request was processed, the response is returned with http status code 200 (OK). 
        /// Otherwise, http status code 404 - NotFound is returned with empty body.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The updated options.</returns>
        public static RpcEndpointOptions ResultAsOkOrNotFound(this RpcEndpointOptions options)
        {
            options.HandlResponse = async (value, context, cancellationToken) =>
            {
                await (value switch
                {
                    SuccessfullyProcessedRequestResult r => HandleSuccessResult(r, context, options, cancellationToken),
                    NotFoundRequestResult r => HandleNotFoundResult(r, context, cancellationToken),
                    RequestNameRouteValueNotFoundResult r => HandleNoRouteResult(r, context, cancellationToken),
                    _ => HandleDefault(value, context, options, cancellationToken)
                });
            };

            return options;
        }

        private static async Task HandleNoRouteResult(RequestNameRouteValueNotFoundResult r, HttpContext context, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentLength = 0L;
            await context.Response.CompleteAsync();
        }

        private static async Task HandleNotFoundResult(NotFoundRequestResult r, HttpContext context, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync($"{r.RequestName} not found", cancellationToken);
        }

        private static async Task HandleSuccessResult(SuccessfullyProcessedRequestResult r, HttpContext context, RpcEndpointOptions options, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await options.SerializeResponse(r.Response, context, cancellationToken);
        }

        private static async Task HandleDefault(object r, HttpContext context, RpcEndpointOptions options, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Accepted;
            await options.SerializeResponse(r, context, cancellationToken);
        }
    }
}