using BenchmarkDotNet.Attributes;

using MediatR.Rpc.AspNetCore.Benchmark.Fakes;
using MediatR.Rpc.AspNetCore.Benchmark.Requests;

using Microsoft.AspNetCore.Http;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Benchmark.Middleware
{
    /// <summary>
    /// Benchmark using proper injections.
    /// </summary>
    public class UsingDefaultConfigurationsWithActualRunner
    {
        //same as MediatR.Rpc.AspNetCore.Known.RouteValues.RequestName.
        private const string RequestName = "RequestName";
        private RequestFactory requestFactory;
        private RpcMiddleware middleware;
        private HttpContext httpContext;
        private string requestName;

        [Params(10, 100, 1000)]
        public int RegistratedRequestsCount;
        

        public UsingDefaultConfigurationsWithActualRunner()
        {
            this.requestFactory = new RequestFactory();
        }

        [GlobalSetup]
        public void Setup()
        {
            var requests = this.requestFactory.TakeRequestTypes(RegistratedRequestsCount).ToList();

            var requestToProcess = requests[RegistratedRequestsCount / 2];
            this.requestName = requestToProcess.Name;

            var runner = new RpcRequestRunner(
                    // As MediatR-RPC only use the non-generic MediatR api, we can just return any type.
                    // Doesn't have to match the corresponding in request and it will not effect the benchmarking.
                    new FakeSender(new RpcBenchmarkResponse()), 
                    new RpcOptions()
                        .UseExactRequestTypeNameMatchingConvention()
                        .ScanRequests(requests));

            this.middleware = new RpcMiddleware(
                new RequestDelegate(c => Task.CompletedTask),
                new RpcEndpointOptions
                {
                    Path = "api"
                }
                .UseSystemJsonForDeserializeBody()
                .UseSystemJsonForOkOrNotFoundResult(),
                new FakeRpcRequestRunner());

            this.httpContext = new FakeHttpContext();
            this.httpContext.Request.RouteValues.Add(RequestName, string.Empty);
            this.httpContext.Request.Body = CreateStreamFromType(requestToProcess, out var contentLength);
            this.httpContext.Request.ContentLength = contentLength;
        }

        [Benchmark]
        public async Task ProcessExistingRequest()
        {
            this.httpContext.Request.RouteValues[RequestName] = this.requestName;
            await this.middleware.Invoke(this.httpContext);
        }

        [Benchmark]
        public async Task ProcessNonExistingRequest()
        {
            this.httpContext.Request.RouteValues.Remove(RequestName);
            await this.middleware.Invoke(this.httpContext);
        }

        private static Stream CreateStreamFromType(Type requestToProcess, out int contentLength)
        {
            var request = Activator.CreateInstance(requestToProcess);
            var json = System.Text.Json.JsonSerializer.Serialize(request, requestToProcess);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);

            contentLength = bytes.Length;
            return new MemoryStream(bytes);
        }

        private class RpcBenchmarkResponse
        {
            public int Number { get; set; } = 9001;
            public string Name { get; set; } = "Name";
        }
    }
}
