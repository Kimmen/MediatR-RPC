using System;
using System.Linq;
using FakeItEasy;
using MediatR.Rpc.AspNetCore.DependencyInjection;
using test.MediatR.Rpc.AspNetCore.Tests.Types;
using Xunit;

namespace MediatR.Rpc.AspNetCore.Tests.DependencyInjection
{
    public class RequestHandlerTypeScannerTest
    {
        [Theory]
        [InlineData(typeof(FlattenRequest))]
        [InlineData(typeof(MyRequest))]
        public void GivenKnownRequestType_WhenFidingRequestTypes_ThenFindsTheExpectedRequest(Type requestType)
        {
            var foundTypes = MediatrTypesScanner
                .ScanForRequestTypes(null, requestType.Assembly)
                .ToArray();

            Assert.Contains(requestType, foundTypes);
        }

        [Fact]
        public void GivenKnownRequestType_WhenSpecifingRequestTypePredicate_ThenPredicateIsCalled()
        {
            var fakePredicate = A.Fake<Func<Type, bool>>();
            A.CallTo(() => fakePredicate(A<Type>.Ignored)).Returns(false);

            MediatrTypesScanner
                .ScanForRequestTypes(fakePredicate, typeof(FlattenRequest).Assembly)
                .ToArray();

            A.CallTo(() => fakePredicate(A<Type>.Ignored)).MustHaveHappened();
        }
    }
}