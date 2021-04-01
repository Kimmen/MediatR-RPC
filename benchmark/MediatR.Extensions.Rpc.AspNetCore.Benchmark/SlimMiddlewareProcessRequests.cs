using BenchmarkDotNet.Attributes;

using MediatR.Rpc.AspNetCore.Benchmark.Fakes;

using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Benchmark
{
    /// <summary>
    /// Only benchmark the actual code in Middleware.,
    /// </summary>
    public class SlimMiddlewareProcessRequests
    {
        //same as MediatR.Rpc.AspNetCore.Known.RouteValues.RequestName.
        private const string RequestName = "RequestName";
        private RpcMiddleware middleware;
        private HttpContext httpContext;


        [GlobalSetup]
        public void Setup()
        {
            this.middleware = new RpcMiddleware(
                new RequestDelegate(c => Task.CompletedTask),
                new RpcEndpointOptions
                {
                    Path = "api",
                    DeserializeRequest = (r, ct) => Task.FromResult(new object()),
                    SerializeResponse = (r, ct) => Task.CompletedTask
                },
                new FakeRpcRequestRunner());

            this.httpContext = new FakeHttpContext();
            this.httpContext.Request.RouteValues.Add(RequestName, string.Empty);
        }

        [Benchmark]
        public async Task ProcessHttpRequestWithRouteValue()
        {
            this.httpContext.Request.RouteValues[RequestName] = $"Request0";
            await this.middleware.Invoke(this.httpContext);
        }

        [Benchmark]
        public async Task ProcessHttpRequestWithoutRouteValue()
        {
            this.httpContext.Request.RouteValues.Remove(RequestName);
            await this.middleware.Invoke(this.httpContext);
        }
    }
}
