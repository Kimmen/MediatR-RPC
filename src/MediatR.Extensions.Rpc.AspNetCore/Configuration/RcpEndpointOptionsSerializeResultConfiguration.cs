using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    public static class RcpEndpointOptionsSerializeResultConfiguration
    {
        private const string JsonContentType = "application/json; charset=utf-8";

        /// <summary>
        /// Given successful result, returns Http status code 200 (OK) and uses <see cref="JsonSerializer"/> for serializing the response body.
        /// Otherwise, http status code 404 - NotFound is returned with an empty body.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="jsonOptions">The Json options used for serialization.</param>
        /// <returns>The updated options.</returns>
        public static RpcEndpointOptions UseSystemJsonForOkOrNotFoundResult(this RpcEndpointOptions options, JsonSerializerOptions? jsonOptions = null)
        {
            options.SerializeResponse = async (response, cancellationToken) =>
            {
                var (result, context) = response;

                await (result switch
                {
                    SuccessfullyProcessedRequestResult r => context
                        .WithStatus(HttpStatusCode.OK)
                        .WithContentType(JsonContentType)
                        .Complete(r.Response, jsonOptions, cancellationToken),

                    NotFoundRequestResult r => context
                        .WithStatus(HttpStatusCode.NotFound)
                        .Complete($"{r.RequestName} not found", jsonOptions, cancellationToken),

                    RequestNameRouteValueNotFoundResult r => context
                        .WithStatus(HttpStatusCode.NotFound)
                        .WithContentType(JsonContentType)
                        .Complete(),

                    _ => context
                        .WithStatus(HttpStatusCode.OK)
                        .Complete(result, jsonOptions, cancellationToken)
                }); ;
            };

            return options;
        }

        private static HttpContext WithStatus(this HttpContext context, HttpStatusCode code)
        {
            context.Response.StatusCode = (int)code;
            return context;
        }

        private static HttpContext WithContentType(this HttpContext context, string contentType)
        {
            context.Response.ContentType = contentType;
            return context;
        }

        private static Task Complete(this HttpContext context, object? responseData, JsonSerializerOptions? options, CancellationToken cancellationToken)
        {
            if(responseData == null)
            {
                return context.Complete();
            }

            var serializedData = JsonSerializer.Serialize(responseData, options);
            context.Response.ContentLength = System.Text.Encoding.UTF8.GetByteCount(serializedData);
            
            return context.Response.WriteAsync(serializedData,cancellationToken);
        }

        private static Task Complete(this HttpContext context)
        {
            context.Response.ContentLength = 0L;
            return context.Response.CompleteAsync();
        }
    }
}