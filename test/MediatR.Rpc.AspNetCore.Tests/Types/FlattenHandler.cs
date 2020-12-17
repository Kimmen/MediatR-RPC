using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace test.MediatR.Rpc.AspNetCore.Tests.Types
{
    public class FlattenResponse { }
    public class FlattenRequest : IRequest<FlattenResponse> { }
    public class FlattenHandler : IRequestHandler<FlattenRequest, FlattenResponse>
    {
        public Task<FlattenResponse> Handle(FlattenRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }



    
}