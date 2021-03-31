using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Configuration
{
    /// <summary>
    /// Standard configurations for the RPC endpoint.
    /// </summary>
    public static class RcpEndpointOptionsDeserializationConfiguration
    {
        /// <summary>
        /// Use <see cref="JsonSerializer"/> for deserializing requests and serializing responses.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="jsonOptions">Optional custom serialization settings.</param>
        /// <returns>The updated options.</returns>
        public static RpcEndpointOptions UseSystemJsonForDeserializeBody(this RpcEndpointOptions options, JsonSerializerOptions? jsonOptions = null)
        {
            jsonOptions ??= new JsonSerializerOptions();

            options.DeserializeRequest = async (request, cancellationToken) =>
            {
                var (requestType, context) = request;

                var hasContent = context.Request.ContentLength > 0;
                return hasContent
                    ? await DeserializeJsonBody(requestType, context.Request.Body, jsonOptions, cancellationToken)
                    : CreateDefault(requestType);

            };

            return options;
        }

        private static async Task<object> DeserializeJsonBody(Type requestType, System.IO.Stream body, JsonSerializerOptions? jsonOptions = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync(body, requestType, jsonOptions, cancellationToken);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Failed to deserialize body as JSON", ex);
            }
        }

        private static object CreateDefault(Type requestType) => Activator.CreateInstance(requestType) ?? throw new InvalidOperationException($"Got null when created request '{requestType.Name}' using default constructor. Nullable requests are not supported.");

    }
}