using System;

using FakeItEasy;

using Mediatr.Rpc;

using MediatR.Extensions.Rpc.Tests.Types;

using Xunit;

namespace MediatR.Extensions.Rpc.Tests.Mapping
{
    public class CustomNameResolver
    {
        [Fact]
        public void GivenKnownTestRequests_WhenSettingACustomNameResolver_ThenTheResolverIsCalled()
        {
            var knownTestTypes = new[] {
                typeof(FlattenRequest),
                typeof(Nested.Request)
            };

            const string requestName = nameof(FlattenRequest);

            var customNameMapResolver = A.Fake<Func<Type, string>>();
            var sut = new LinearSearchRequestTypeProvider(knownTestTypes, customNameMapResolver);

            sut.TryGetByName(requestName, out var foundType);

            A.CallTo(() => customNameMapResolver).MustHaveHappened();
        }
    }
}