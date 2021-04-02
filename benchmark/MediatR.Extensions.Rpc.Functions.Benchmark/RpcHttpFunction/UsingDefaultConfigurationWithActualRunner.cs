
using BenchmarkDotNet.Attributes;

using MediatR.Rpc.Azure.Functions;
using MediatR.Rpc.Functions.Benchmark.Fakes;
using MediatR.Rpc.Functions.Benchmark.Requests;

using Microsoft.AspNetCore.Http;

using System.Linq;
using System.Threading.Tasks;

namespace MediatR.Rpc.Functions.Benchmark
{
    public class UsingDefaultConfigurationWithActualRunner
    {
        private RpcHttpFunction httpCaller;
        private HttpRequest httpRequest;
        private string requestName;
        private RequestFactory requestFactory;

        [Params(10, 100, 1000)]
        public int RegistratedRequestsCount;

        public UsingDefaultConfigurationWithActualRunner()
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

            this.httpCaller = new RpcHttpFunction(
                new RpcHttpFunctionOptions()
                    .UseNewtonsoftJsonForDeserializeBody()
                    .UseOkorNotFoundActionResults(), 
                runner);

            this.httpRequest = new FakeHttpRequest();
        }

        [Benchmark]
        public async Task ProcessHttpRequest()
        {
            await this.httpCaller.ProcessHttpRequest(this.requestName, this.httpRequest);
        }

        private class RpcBenchmarkResponse
        {
            public int Number { get; set; } = 9001;
            public string Name { get; set; } = "Name";
        }
    }
}
