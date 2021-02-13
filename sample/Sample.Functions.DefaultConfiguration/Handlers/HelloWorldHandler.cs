using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sample.Functions.DefaultConfiguration.Handlers
{
    public class HelloWorldResponse
    {
        public string Message { get; set; }
    }

    public class HelloWorldRequest : IRequest<HelloWorldResponse>
    {
        public string Name { get; set; }
    }
    public class HelloWorldHandler : IRequestHandler<HelloWorldRequest, HelloWorldResponse>
    {
        public Task<HelloWorldResponse> Handle(HelloWorldRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HelloWorldResponse
            {
                Message = $"Hello world, {request.Name}"
            });
        }
    }
}