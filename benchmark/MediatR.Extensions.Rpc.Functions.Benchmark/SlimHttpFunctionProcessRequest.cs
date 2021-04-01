
using BenchmarkDotNet.Attributes;

using MediatR.Rpc.Azure.Functions;
using MediatR.Rpc.Functions.Benchmark.Fakes;
using MediatR.Rpc.Functions.Benchmark.Requests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Threading.Tasks;

namespace MediatR.Rpc.Functions.Benchmark
{
    public class SlimHttpFunctionProcessRequest
    {
        private RpcHttpFunction httpCaller;

        public int RegistratedRequestsCount;
        private HttpRequest httpRequest;

        [GlobalSetup]
        public void Setup()
        {
            //Include the proper implementation in the benchmark for more realistic result.
            //this.runner = new RpcRequestRunner(new FakeSender(), new RpcOptions
            //{
            //    MatchingConvention = (t) => t.Name,
            //    Requests = this.requestFactory.TakeRequestTypes(RegistratedRequestsCount).ToList()
            //});

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
