using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sample.Functions.DefaultConfiguration.Handlers
{
    public class HelloWorldResponse
    {
        public string Message { get; set; }
    }

    public class HelloWorld : IRequest<HelloWorldResponse>
    {
        public string Name { get; set; }
    }
    public class HelloWorldHandler : IRequestHandler<HelloWorld, HelloWorldResponse>
    {
        public Task<HelloWorldResponse> Handle(HelloWorld request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HelloWorldResponse
            {
                Message = $"Hello world, {request.Name}"
            });
        }
    }
}