using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MediatR.Rpc.AspNetCore
{
    public static class RcpEndpointOptionsConfigurator
    {
        public static RpcEndpointOptions SerializeWithSystemJson(this RpcEndpointOptions options, JsonSerializerOptions jsonOptions)
        {
            jsonOptions ??= new JsonSerializerOptions();

            options.DeserializeRequest = async (t, s, cancellationToken) =>
            {
                var hasContent = s.ContentLength > 0;
                
                if(hasContent)
                {
                    try
                    {
                        return await JsonSerializer.DeserializeAsync(s.Body, t, jsonOptions, cancellationToken);
                    }
                    catch (JsonException ex)
                    {
                        throw new ArgumentException("Failed to deserialize body as JSON", ex);
                    }
                }

                //If there is no body, let's create is using default constructor. 
                return Activator.CreateInstance(t);
            };
            options.SerializeResponse = (v) => JsonSerializer.Serialize(v, jsonOptions);

            return options;
        }

        public static RpcEndpointOptions UnmatchedReqeustsAs404NotFound(this RpcEndpointOptions options)
        {
            options.UnmatchedRequest = (requestName, context, cancellationToken) =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Task.CompletedTask;
            };

            return options;
        }

        public static RpcEndpointOptions ResponsesAs200Ok(this RpcEndpointOptions options)
        {
            options.HandlResponse = async (value, context, cancellationToken) =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(value, cancellationToken);
            };

            return options;
        }
    }
}