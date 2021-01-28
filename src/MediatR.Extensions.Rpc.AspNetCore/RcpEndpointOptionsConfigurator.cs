using System;
using System.Net;
using System.Text.Json;
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
        /// Use <see cref="System.Text.Json.JsonSerializer"/> for deserializing requests and serializing responses.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="jsonOptions">Optional custom serialization settings.</param>
        /// <returns>The updated options.</returns>
        public static RpcEndpointOptions SerializeWithSystemJson(this RpcEndpointOptions options, JsonSerializerOptions? jsonOptions = null)
        {
            jsonOptions ??= new JsonSerializerOptions();

            options.DeserializeRequest = async (t, c, cancellationToken) =>
            {
                var request = c.Request;
                var hasContent = request.ContentLength > 0;
                
                if(hasContent)
                {
                    try
                    {
                        return await JsonSerializer.DeserializeAsync(request.Body, t, jsonOptions, cancellationToken);
                    }
                    catch (JsonException ex)
                    {
                        throw new ArgumentException("Failed to deserialize body as JSON", ex);
                    }
                }

                //If there is no body, let's create is using default constructor. 
                return Activator.CreateInstance(t);
            };

            options.SerializeResponse = (v, r, ct) =>
            {
                return Task.FromResult(JsonSerializer.Serialize(v, jsonOptions));
            };

            return options;
        }

        /// <summary>
        /// Respond any unmatched requests with HttpStatus 404 - NotFound.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The updated options.</returns>
        public static RpcEndpointOptions UnmatchedRequestsAs404NotFound(this RpcEndpointOptions options)
        {
            options.UnmatchedRequest = (requestName, context, cancellationToken) =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Task.CompletedTask;
            };

            return options;
        }

        /// <summary>
        /// Respond any successful requests with HttpStatus 200 - OK and serialized response object.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The updated options.</returns>
        public static RpcEndpointOptions ResponsesAs200Ok(this RpcEndpointOptions options)
        {
            options.HandlResponse = async (value, context, cancellationToken) =>
            {
                var body = await options.SerializeResponse(value, context, cancellationToken);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(body, cancellationToken);
            };

            return options;
        }
    }
}