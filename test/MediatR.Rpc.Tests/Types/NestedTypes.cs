using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace test.Types
{
    public static class Nested
    {
        public class Response
        {

        }
        public class Request : IRequest<Response>
        {

        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Response());
            }
        }
    }
}