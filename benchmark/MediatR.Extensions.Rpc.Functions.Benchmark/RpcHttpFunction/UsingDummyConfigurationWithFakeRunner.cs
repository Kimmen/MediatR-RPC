
using BenchmarkDotNet.Attributes;

using MediatR.Rpc.Azure.Functions;
using MediatR.Rpc.Functions.Benchmark.Fakes;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace MediatR.Rpc.Functions.Benchmark
{
    public class UsingDummyConfigurationWithFakeRunner
    {
        private RpcHttpFunction httpCaller;

        public int RegistratedRequestsCount;
        private HttpRequest httpRequest;

        [GlobalSetup]
        public void Setup()
        {
            IActionResult serializationResult = new OkResult();
            this.httpCaller = new RpcHttpFunction(new RpcHttpFunctionOptions
            {
                DeserializeRequest = (r, ct) => Task.FromResult(new object()),
                SerializeResponse = (r, ct) => Task.FromResult(serializationResult)
            }, new FakeRpcRequestRunner());

            this.httpRequest = new FakeHttpRequest();
        }

        [Benchmark]
        public async Task ProcessHttpRequest()
        {
            var requestName = $"Request{RegistratedRequestsCount / 2}";
            await this.httpCaller.ProcessHttpRequest(requestName, this.httpRequest);
        }
    }
}
