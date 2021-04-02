using FluentAssertions;

using MediatR.Rpc.Core.Tests.Types;

using Xunit;

namespace MediatR.Rpc.Core.Tests.RequestTypeScanner
{
    public class IsOpenGenericTests
    {
        [Fact]
        public void GivenClosedGeneric_WhenEvaluating_ThenIsDeterminedNotToBeOpened()
        {
            var isOpenGeneric = Rpc.RequestTypeScanner.IsOpenGeneric(typeof(FlattenRequest));

            isOpenGeneric.Should().BeFalse();
        }

        [Fact]
        public void GivenOpenGeneric_WhenEvaluating_ThenIsDeterminedNotToBeOpened()
        {
            var isMediatrRequest = Rpc.RequestTypeScanner.IsOpenGeneric(typeof(OpenGenericRequest<>));

            isMediatrRequest.Should().BeTrue();
        }
    }
}
