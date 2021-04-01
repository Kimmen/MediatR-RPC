using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;

using MediatR.Rpc.Benchmark;
using MediatR.Rpc.Benchmark.Requests;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Core.Benchmark
{
#nullable disable
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
        public async Task<IRpcResult> ProcessRequest()
        {
            var target = nameof(Request0);
            return await this.runner.Process(target, this.ValueFactory, default);
        }

        private Task<object> ValueFactory(System.Type targetType, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }
    }
#nullable restore
}
