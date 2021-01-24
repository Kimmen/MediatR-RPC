using MediatR;
using MediatR.Rpc.AspNetCore;
using MediatR.Rpc.AspNetCore.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Sample.AspNetCore.CustomConfiguration.Configuration;

namespace Sample.AspNetCore.CustomConfiguration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var targetAssembly = this.GetType().Assembly;
            services.AddMediatR(targetAssembly);
            services.AddMediatrRpc(o =>
            {
                o.ScanRequests(targetAssembly);
                o.UseAppsCustomNameMapping();
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRpc(o =>
                {
                    o.Path = "api";
                    o.SerializeWithJsonNet(new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }) ;
                    o.HandleCommonAppResponse();
                });
            });
        }
    }
}
