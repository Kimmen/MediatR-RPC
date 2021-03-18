using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    /// <summary>
    /// Standard configurations for the RPC endpoint.
    /// </summary>
    public static class RcpEndpointOptionsConfigurator
    {
        /// <summary>
        /// Use <see cref="JsonSerializer"/> for deserializing requests and serializing responses.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="jsonOptions">Optional custom serialization settings.</param>
        /// <returns>The updated options.</returns>
        public static RpcEndpointOptions SerializeWithSystemJson(this RpcEndpointOptions options, JsonSerializerOptions? jsonOptions = null)
        {
            jsonOptions ??= new JsonSerializerOptions();

            options.DeserializeRequest = async (requestType, context, cancellationToken) =>
            {
                var request = context.Request;
                var hasContent = request.ContentLength > 0;
                
                if(hasContent)
                {
                    try
                    {
                        return await JsonSerializer.DeserializeAsync(request.Body, requestType, jsonOptions, cancellationToken);
                    }
                    catch (JsonException ex)
                    {
                        throw new ArgumentException("Failed to deserialize body as JSON", ex);
                    }
                }

                //If there is no body, let's create is using default constructor. 
                return Activator.CreateInstance(requestType);
            };

            options.SerializeResponse = async (response, context, cancellationToken) =>
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions), cancellationToken);
            };

            return options;
        }

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
                    _ => throw new NotImplementedException()
                });
            };

            return options;
        }

        private static async Task HandleNotFoundResult(NotFoundRequestResult r, HttpContext context, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            if(string.IsNullOrWhiteSpace(r.RequestName))
            {
                await context.Response.WriteAsync($"{r.RequestName} not found", cancellationToken);
            }
        }

        private static async Task HandleSuccessResult(SuccessfullyProcessedRequestResult r, HttpContext context, RpcEndpointOptions options, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await options.SerializeResponse(r.Response, context, cancellationToken);
        }
    }
}