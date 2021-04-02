
using FluentAssertions;

using System.Threading.Tasks;

using Xunit;

namespace MediatR.Rpc.Core.Tests.RpcRequestRunner
{
    public class WhenProcessingKnownRequest
    {
        private RpcRunnerFixture fixture = new RpcRunnerFixture();
        public WhenProcessingKnownRequest()
        {
            fixture
                .WithRegistratedRequestTypes(
                    typeof(Types.FlattenRequest),
                    typeof(Types.Nested.Request))
                .WithResponse(new Types.FlattenResponse())
                .WithRequest(new Types.FlattenRequest());
        }

        [Fact]
        public async Task ThenSuccessResultIsReturned()
        {
            var result = await fixture.Process();

            result.Should().BeOfType<SuccessfullyProcessedRequestResult>();
        }

        [Fact]
        public async Task ThenResultObjectIsExpectedType()
        {
            var result = await fixture.Process();

            var response = ((SuccessfullyProcessedRequestResult)result).Response;
            response.Should().BeOfType<Types.FlattenResponse>();
        }
    }
}
