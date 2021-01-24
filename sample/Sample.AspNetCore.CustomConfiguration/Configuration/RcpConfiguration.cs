using Mediatr.Rpc;

using MediatR.Rpc.AspNetCore;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Sample.AspNetCore.CustomConfiguration.Handlers;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

            options.SerializeResponse = (response, httpContext, cancellationToken) =>
            {
                return Task.FromResult(JsonConvert.SerializeObject(response));
            };

            return options;
        }

        /// <summary>
        /// As we are using a custom common app response for all handlers, we can commonly handle special logic here.
        /// </summary>
        public static RpcEndpointOptions HandleCommonAppResponse(this RpcEndpointOptions options)
        {
            options.HandlResponse = async (response, httpContext, cancellationToken) =>
            {
                if(response is CommonAppResponse resp)
                {
                    if(resp.ValidationErrors.Any())
                    {
                        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                        var errors = await options.SerializeResponse(resp.ValidationErrors, httpContext, cancellationToken);
                        await httpContext.Response.WriteAsync(errors, cancellationToken);
                    }
                    if(resp.Response == null)
                    {
                        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                    }
                    
                    httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    var body = await options.SerializeResponse(resp.Response, httpContext, cancellationToken);
                    await httpContext.Response.WriteAsync(body, cancellationToken);
                }
                else
                {
                    httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                }
            };

            options.UnmatchedRequest = (requstName, httpContext, cancellationToken) =>
            {
                httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Task.CompletedTask;
            };

            return options;
        }
    }
}
