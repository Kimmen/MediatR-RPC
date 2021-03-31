using MediatR;
using MediatR.Rpc;
using MediatR.Rpc.Azure.Functions;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

using Sample.Functions.DefaultConfiguration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sample.Functions.DefaultConfiguration
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var targetAssembly = this.GetType().Assembly;

            builder.Services
                .AddMediatR(targetAssembly)
                .AddMediatrRpc(o =>
                {
                    o.ScanRequests(targetAssembly);
                    o.UseExactRequestTypeNameMatchingConvention();
                })
                .AddMediatrRpcHttp(o => 
                {
                    o.UseNewtonsoftJsonForDeserializeBody();
                    o.UseOkorNotFoundActionResults();
                });
        }
    }
}
