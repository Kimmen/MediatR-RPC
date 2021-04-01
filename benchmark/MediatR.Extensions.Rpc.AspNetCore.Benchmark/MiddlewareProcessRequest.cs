using BenchmarkDotNet.Attributes;

using MediatR.Rpc.AspNetCore.Benchmark.Requests;

using Microsoft.AspNetCore.Http;

using System.Linq;
using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Benchmark
{
    public class MiddlewareProcessRequest
    {
        //same as MediatR.Rpc.AspNetCore.Known.RouteValues.RequestName.
        private const string RequestName = "RequestName";
        private readonly RequestFactory requestFactory;
        private RpcRequestRunner runner;
        private RpcMiddleware middleware;
        private HttpContext httpContext;

        [Params(10, 100, 1000)]
        public int RegistratedRequestsCount;

        public MiddlewareProcessRequest()
        {
            this.requestFactory = new RequestFactory();
        }

        [GlobalSetup]
        public void Setup()
        {
            //Include the proper implementation in the benchmark for more realistic result.
            this.runner = new RpcRequestRunner(new FakeSender(), new RpcOptions
            {
                MatchingConvention = (t) => t.Name,
                Requests = this.requestFactory.TakeRequestTypes(RegistratedRequestsCount).ToList()
            });

            this.middleware = new RpcMiddleware(
                new RequestDelegate(c => Task.CompletedTask),
                new RpcEndpointOptions
                {
                    Path = "api",
                    DeserializeRequest = (r, ct) => Task.FromResult(new object()),
                    SerializeResponse = (r, ct) => Task.CompletedTask
                },
                this.runner);

            this.httpContext = new FakeHttpContext();
            this.httpContext.Request.RouteValues.Add(RequestName, string.Empty);
        }

        [Benchmark]
        public async Task ProcessHttpRequestWithExistingRequestNameRouteValue()
        {
            this.httpContext.Request.RouteValues[RequestName] = $"Request{RegistratedRequestsCount / 2}";
            await this.middleware.Invoke(this.httpContext);
        }

        [Benchmark]
        public async Task ProcessHttpRequestWithNontExistingRequestNameRouteValue()
        {
            this.httpContext.Request.RouteValues[RequestName] = "RequestNotExisting";
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
