using System;
using Mediatr.Rpc;
using Xunit;
using test.Types;

namespace test.Probing
{
    public class ReflectedRequestTypeProviderTests
    {
        [Theory]
        [InlineData(typeof(FlattenRequest))]
        [InlineData(typeof(Nested.Request))]
        public void GivenKnownTestRequest_WhenScanning_ThenTargetTypeIsFound(Type knownRequestType)
        {
            var foundRequestTypes = ReflectionTypeScanner.FindRequestTypes(knownRequestType.Assembly);

            Assert.Contains(knownRequestType, foundRequestTypes);
        }
    }
}
