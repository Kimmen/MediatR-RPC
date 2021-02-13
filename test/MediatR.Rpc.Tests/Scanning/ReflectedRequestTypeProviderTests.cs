using System;
using Mediatr.Rpc;

using MediatR.Extensions.Rpc.Tests.Types;

using Xunit;

namespace MediatR.Extensions.Rpc.Tests.Scanning
{
    public class ReflectedRequestTypeProviderTests
    {
        [Theory]
        [InlineData(typeof(FlattenRequest))]
        [InlineData(typeof(Nested.Request))]
        public void GivenKnownTestRequest_WhenScanning_ThenTargetTypeIsFound(Type knownRequestType)
        {
            var foundRequestTypes = RequestTypeScanner.FindRequestTypes(new[] { knownRequestType });

            Assert.Contains(knownRequestType, foundRequestTypes);
        }
    }
}
