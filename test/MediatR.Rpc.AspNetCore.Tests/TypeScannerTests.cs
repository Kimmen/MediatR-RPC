//using System;

//using test.MediatR.Rpc.AspNetCore.Tests.Types;

//using Xunit;

//using Enumerable = System.Linq.Enumerable;

//namespace MediatR.Rpc.AspNetCore.Tests
//{
//    public class TypeScannerTests
//    {
//        [Fact]
//        public void GivenTestRequestHandler_WhenScanningTypes_ThenRequestsAreFound()
//        {
//            var handlerTypes = new[] {
//                typeof(FlattenHandler)
//            };

//            var requests = TypeScanner.GetMediatrRequests(handlerTypes);
//            Assert.Contains(typeof(FlattenRequest), requests);
//        }
//    }
//}
