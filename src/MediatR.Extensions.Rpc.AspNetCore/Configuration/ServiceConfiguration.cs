using System;

using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Rpc.AspNetCore.Configuration
{
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Adds services for the MediatR RPC behavior, to enable sending <see cref="IRequest{TResponse}"/> given a corresponding name and payload.
        /// </summary>
        /// <param name="services">Service collection to register to.</param>
        /// <param name="configuration">The configurator for the necessary options.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddMediatrRpc(this IServiceCollection services, Action<RpcOptions> configuration)
        {
            Validate(services, configuration);

            var options = new RpcOptions();
            configuration.Invoke(options);

            services.AddSingleton(options);
            services.AddSingleton<RpcRequestRunner>();
            services.AddSingleton<IRpcRequestRunner>(provider => provider.GetService<RpcRequestRunner>());

            return services;
        }

        private static void Validate(IServiceCollection services, Action<RpcOptions> configuration)
        {
            AssertHelper.ValidateIsNotNull(services, nameof(services));
            AssertHelper.ValidateIsNotNull(configuration, nameof(configuration));
        }
    }
}