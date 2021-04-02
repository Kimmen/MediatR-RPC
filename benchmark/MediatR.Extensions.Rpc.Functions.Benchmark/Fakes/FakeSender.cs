using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Functions.Benchmark.Fakes
{
    public sealed class FakeSender : ISender
    {
        private readonly object defaultUntypedResponse;

        public FakeSender(object defaultUntypedResponse = null)
        {
            this.defaultUntypedResponse = defaultUntypedResponse;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var response = default(TResponse);
            return Task.FromResult(response);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.defaultUntypedResponse);
        }
    }
}
