using System;
using System.Diagnostics.CodeAnalysis;
using Mediatr.Rpc;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Rpc.AspNetCore
{
    public static class EndpointConfig
    {
        public static void MapRpc(this IEndpointRouteBuilder builder, [NotNull] Action<RpcEndpointOptions> configuration)
        {
            Validate(builder, configuration);
            var options = BuildOptions(configuration);
            Validate(options);

            var pattern = options.Path + "/{" + options.RequestNameRouteKey + ":alpha}";

            var rpcCaller = builder.ServiceProvider.GetService<RpcCallExecuter>();

            var pipeline = builder.CreateApplicationBuilder()
                .UseMiddleware<MediatrRpcMiddleware>(options, rpcCaller)
                .Build();

            builder
                .Map(pattern, pipeline);
        }

        private static RpcEndpointOptions BuildOptions(Action<RpcEndpointOptions> configuration)
        {
            var options = new RpcEndpointOptions();
            configuration?.Invoke(options);
            return options;
        }

        private static void Validate(RpcEndpointOptions options)
        {
            //TODO: Maybe need some validation afterwards?
        }

        private static void Validate(IEndpointRouteBuilder builder, Action<RpcEndpointOptions> configuration)
        {
            if(builder is null)
            {
                throw new ArgumentNullException(nameof(builder), $"The {nameof(IEndpointRouteBuilder)} was not provided.");
            }

            if(configuration  is null)
            {
                throw new ArgumentNullException(nameof(configuration), $"Configuration method was not provided.");
            }
        }
    }
}