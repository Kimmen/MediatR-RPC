using System;

using Mediatr.Rpc;

using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Rpc.AspNetCore.DependencyInjection
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddMediatrRpc(this IServiceCollection services, Action<RpcOptions> configuration)
        {
            Validate(services, configuration);

            var options = new RpcOptions();
            configuration.Invoke(options);

            services.AddSingleton(options);
            services.AddSingleton<RpcCallExecuter>();

            return services;
        }

        private static void Validate(IServiceCollection services, Action<RpcOptions> configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services), "ServiceCollection can not be null.");
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration), $"Option configuration can not be null.");
            }
        }
    }
}