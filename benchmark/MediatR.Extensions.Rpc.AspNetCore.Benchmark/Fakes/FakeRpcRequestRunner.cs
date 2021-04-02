
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Benchmark.Fakes
{
    public sealed class FakeRpcRequestRunner : IRpcRequestRunner
    {
        private static readonly IRpcResult DefaultResult = new SuccessfullyProcessedRequestResult
        {
            Response = new object()
        };

        public Task<IRpcResult> Process(string requestName, Func<Type, CancellationToken, Task<object>> requestValueFactory, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(DefaultResult);
        }
    }
}
