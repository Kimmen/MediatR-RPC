﻿using Microsoft.Extensions.DependencyInjection;

using System;

namespace MediatR.Rpc.Azure.Functions
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

        public static IServiceCollection AddMediatrRpcHttp(this IServiceCollection services, Action<RpcHttpFunctionOptions> configuration)
        {
            Validate(services, configuration);
            var options = new RpcHttpFunctionOptions();
            configuration.Invoke(options);

            services.AddSingleton(options);
            services.AddTransient<IRpcHttpFunction, RpcHttpFunction>();

            return services;
        }

        private static void Validate<T>(IServiceCollection services, Action<T> configuration)
        {
            AssertHelper.ValidateIsNotNull(services, nameof(services));
            AssertHelper.ValidateIsNotNull(configuration, nameof(configuration));
        }
    }
}
