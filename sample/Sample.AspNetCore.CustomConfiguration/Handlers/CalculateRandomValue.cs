using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.AspNetCore.CustomConfiguration.Handlers
{
    public static class CalculateRandomValue
    {
        public class Handler : IRequestHandler<Request, CommonAppResponse>
        {
            public async Task<CommonAppResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.Seed < 100)
                {
                    return RespondWith.ValidationErrors(new ValidationError()
                    {
                        Code = 1,
                        Message = "Seed need to be larger than 100"
                    });
                }

                var randomizer = new Random(request.Seed);
                //Don't mind this quirky await from result.
                var calculatedValue = await Task.FromResult(randomizer.NextDouble());

                return RespondWith.Ok(calculatedValue);
            }
        }

        public class Request : IAppRequest
        {
            public int Seed { get; set; }
        }
    }
}
