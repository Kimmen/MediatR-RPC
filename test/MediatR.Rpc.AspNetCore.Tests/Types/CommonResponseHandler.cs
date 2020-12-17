using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace test.MediatR.Rpc.AspNetCore.Tests.Types
{
    public class CommonResponse
    {
        public object Response { get; }

        public bool IsSuccess { get; }
    }

    public interface ICommonRequest : IRequest<CommonResponse> { }

    public class MyRequest : ICommonRequest { }

    public class MyHandler : IRequestHandler<MyRequest, CommonResponse>
    {
        public Task<CommonResponse> Handle(MyRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}