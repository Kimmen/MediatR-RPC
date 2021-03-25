using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Rpc.AspNetCore
{
    /// <summary>
    /// Endpoint configuration for routing the RPC api.
    /// </summary>
    public static class EndpointConfig
    {
        /// <summary>
        /// Adds the RPC endpoint to the <see cref="IEndpointRouteBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder to add the endpoint to.</param>
        /// <param name="configuration">Configurator for the options needed for the endpoint.</param>
        /// <returns>The updated builder.</returns>
        public static IEndpointRouteBuilder MapRpc(this IEndpointRouteBuilder builder, [NotNull] Action<RpcEndpointOptions> configuration)
        {
            Validate(builder, configuration);

            var options = BuildOptions(configuration);
            var pattern = options.Path + "/{" + Known.RouteValues.RequestName + ":alpha}";
            var rpcCaller = builder.ServiceProvider.GetService<IRpcRequestRunner>();

            var pipeline = builder
                .CreateApplicationBuilder()
                .UseMiddleware<RpcMiddleware>(options, rpcCaller)
                .Build();

            builder
                .Map(pattern, pipeline);

            return builder;
        }

        private static RpcEndpointOptions BuildOptions(Action<RpcEndpointOptions> configuration)
        {
            var options = new RpcEndpointOptions();
            configuration?.Invoke(options);
            return options;
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