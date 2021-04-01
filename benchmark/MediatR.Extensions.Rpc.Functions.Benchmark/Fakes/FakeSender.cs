using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Functions.Benchmark.Fakes
{
    public sealed class FakeSender : ISender
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var response = default(TResponse);
            return Task.FromResult(response);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            object response = null;
            return Task.FromResult(response);
        }
    }
}
