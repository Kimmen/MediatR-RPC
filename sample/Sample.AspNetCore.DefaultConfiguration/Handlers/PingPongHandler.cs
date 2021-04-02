using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sample.AspNetCore.DefaultConfiguration.Handlers
{
    public class Pong
    {
        public string Message { get; set; }
    }

    public class Ping : IRequest<Pong>
    {
        public string Message { get; set; }
    }

    public class PingPongHandler : IRequestHandler<Ping, Pong>
    {
        public Task<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Pong
            {
                Message = request.Message
            });
        }
    }
}