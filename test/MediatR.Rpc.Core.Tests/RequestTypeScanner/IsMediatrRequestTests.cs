using FluentAssertions;

using MediatR.Rpc.Core.Tests.Types;

using Xunit;

namespace MediatR.Rpc.Core.Tests.RequestTypeScanner
{
    public class IsMediatrRequestTests
    {
        [Fact]
        public void GivenMediatrRequest_WhenEvaluating_ThenIsDeterminedToBeAMediatrRequest()
        {
            var isMediatrRequest = Rpc.RequestTypeScanner.IsMediatrRequest(typeof(FlattenRequest));

            isMediatrRequest.Should().BeTrue();
        }

        [Fact]
        public void GivenNonMediatrRequest_WhenEvaluating_ThenIsDeterminedNotToBeAMediatrRequest()
        {
            var isMediatrRequest = Rpc.RequestTypeScanner.IsMediatrRequest(typeof(NormalType));

            isMediatrRequest.Should().BeFalse();
        }
    }
}
