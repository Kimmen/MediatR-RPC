
using FluentAssertions;

using System.Threading.Tasks;

using Xunit;

namespace MediatR.Rpc.Core.Tests.RpcRequestRunner
{
    public class WhenProcessingUnmappedRequest
    {
        private RpcRunnerFixture fixture = new RpcRunnerFixture();
        public WhenProcessingUnmappedRequest()
        {
            fixture
                .WithRegistratedRequestTypes(
                    typeof(Types.Nested.Request))
                .WithRequest(new Types.FlattenRequest());
        }

        [Fact]
        public async Task ThenNotFoundResultIsReturned()
        {
            var result = await fixture.Process();

            result.Should().BeOfType<NotFoundRequestResult>();
        }

        [Fact]
        public async Task ThenNotFoundResultHasRequestName()
        {
            var result = await fixture.Process();

            var notFoundResult = ((NotFoundRequestResult)result);
            notFoundResult.RequestName.Should().Be(nameof(Types.FlattenRequest));
        }
    }
}
