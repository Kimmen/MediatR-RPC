using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Core.Tests.Types
{
    public class FlattenRequest : IRequest<FlattenResponse>
    {

    }

    public class FlattenResponse
    {

    }

    public class FlattenHandler : IRequestHandler<FlattenRequest, FlattenResponse>
    {
        public Task<FlattenResponse> Handle(FlattenRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FlattenResponse());
        }
    }
}
