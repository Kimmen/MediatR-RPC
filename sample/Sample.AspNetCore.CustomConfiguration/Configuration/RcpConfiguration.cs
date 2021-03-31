using MediatR.Rpc;
using MediatR.Rpc.AspNetCore;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Sample.AspNetCore.CustomConfiguration.Handlers;

using System;
using System.IO;
using System.Linq;

namespace Sample.AspNetCore.CustomConfiguration.Configuration
{
    public static class RcpConfiguration
    {
        /// <summary>
        /// Our requests and requests handlers are nested together in one static class.
        /// It's the static class' name we want to use when routing.
        /// </summary>
        public static RpcOptions UseAppsCustomNameMapping(this RpcOptions options)
        {
            options.MatchingConvention = requestType =>
            {
                return requestType.DeclaringType.Name.ToLowerInvariant();
            };

            return options;
        }

        /// <summary>
        /// Here we use custom serialization code, in this case just use NewtonSoft's Json.NET.
        /// </summary>
        public static RpcEndpointOptions SerializeWithJsonNet(this RpcEndpointOptions options, JsonSerializerSettings settings)
        {
            options.DeserializeRequest = async (targetType, httpContext, cancellationToken) =>
            {
                var request = httpContext.Request;

                if(request.ContentLength > 0)
                {
                    using var reader = new StreamReader(request.Body);
                    var body = await reader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject(body, targetType, settings);
                }

                return Activator.CreateInstance(targetType);
            };

            options.SerializeResponse = async (response, httpContext, cancellationToken) =>
            {
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), cancellationToken);
            };

            return options;
        }

        /// <summary>
        /// As we are using a custom common app response for all handlers, we can commonly handle special logic here.
        /// </summary>
        public static RpcEndpointOptions HandleCommonAppResponse(this RpcEndpointOptions options)
        {
            options.HandlResponse = async (result, httpContext, cancellationToken) =>
            {
                if(result is SuccessfullyProcessedRequestResult succResult)
                {
                    if (succResult.Response is CommonAppResponse response)
                    {
                        if (response.ValidationErrors.Any())
                        {
                            httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                            await options.SerializeResponse(response.ValidationErrors, httpContext, cancellationToken);

                        }
                        if (response.Response == null)
                        {
                            httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                        }

                        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        await options.SerializeResponse(response.Response, httpContext, cancellationToken);
                    }
                    else
                    {
                        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    }
                }
                else  if(result is NotFoundRequestResult notFoundResult)
                {
                    httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    await httpContext.Response.WriteAsync(notFoundResult.RequestName, cancellationToken);
                }
            };

            return options;
        }
    }
}
