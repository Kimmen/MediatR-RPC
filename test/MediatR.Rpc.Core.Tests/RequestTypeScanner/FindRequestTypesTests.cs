using System;

using FluentAssertions;

using MediatR.Rpc.Core.Tests.Types;

using Xunit;

namespace MediatR.Rpc.Core.Tests.RequestTypeScanner
{
    public class FindRequestTypesTests
    {
        [Theory]
        [InlineData(typeof(FlattenRequest))]
        [InlineData(typeof(Nested.Request))]
        public void GivenKnownTestRequest_WhenScanning_ThenTargetTypeIsFound(Type knownRequestType)
        {
            var foundRequestTypes = Rpc.RequestTypeScanner.FindRequestTypes(new[] { knownRequestType });

            foundRequestTypes.Should().Contain(knownRequestType);
        }

        [Fact]
        public void GivenNonRequestTypes_WhenScanning_ThenNoRequestTypesAreFound()
        {
            //Some types that are not mediatr requests.
            var nonRequestTypes = new[]  
            {
                GetType(),
                typeof(int),
                typeof(string), 
                typeof(DateTime),
                typeof(System.Collections.ArrayList),
                typeof(NormalType)
            };

            var foundRequestTypes = Rpc.RequestTypeScanner.FindRequestTypes(nonRequestTypes);

            foundRequestTypes.Should().BeEmpty();
        }
    }
}
