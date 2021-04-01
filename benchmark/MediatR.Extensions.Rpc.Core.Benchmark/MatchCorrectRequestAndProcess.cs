using BenchmarkDotNet.Attributes;

using MediatR.Rpc.Benchmark;
using MediatR.Rpc.Benchmark.Requests;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Core.Benchmark
{
    public class MatchCorrectRequestAndProcess
    {
        private static readonly object request = new Request0();
        private readonly RequestFactory requestFactory;
        private RpcRequestRunner runner;

        [Params(10, 100, 1000)]
        public int RegistratedRequestsCount;

        public MatchCorrectRequestAndProcess()
        {
            this.requestFactory = new RequestFactory();
        }

        [GlobalSetup]
        public void Setup()
        {
            RpcOptions options = new()
            {
                MatchingConvention = (t) => t.Name,
                Requests = this.requestFactory.TakeRequestTypes(RegistratedRequestsCount).ToList()
            };

            this.runner = new RpcRequestRunner(new FakeSender(), options);
        }

        [Benchmark]
        public async Task<IRpcResult> MatchFirst()
        {
            var target = "Request0";
            return await this.runner.Process(target, this.ValueFactory, default);
        }

        [Benchmark]
        public async Task<IRpcResult> MatchMiddle()
        {
            var target = $"Request{RegistratedRequestsCount / 2}";
            return await this.runner.Process(target, this.ValueFactory, default);
        }

        [Benchmark]
        public async Task<IRpcResult> MatchLast()
        {
            var target = $"Request{RegistratedRequestsCount - 1}";
            return await this.runner.Process(target, this.ValueFactory, default);
        }

        [Benchmark]
        public async Task<IRpcResult> NoMatch()
        {
            var target = $"NoExistingRequest";
            return await this.runner.Process(target, this.ValueFactory, default);
        }

        private Task<object> ValueFactory(System.Type targetType, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }
    }
}
