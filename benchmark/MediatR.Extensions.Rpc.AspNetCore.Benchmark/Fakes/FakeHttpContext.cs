using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace MediatR.Rpc.AspNetCore.Benchmark.Fakes
{
    public sealed class FakeHttpContext : HttpContext
    {
        private HttpRequest request = new FakeHttpRequest();
        private HttpResponse response = new FakeHttpResponse();

        public override IFeatureCollection Features => new FeatureCollection();

        public override HttpRequest Request => request;

        public override HttpResponse Response => response;

        public override ConnectionInfo Connection => throw new NotImplementedException();

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CancellationToken RequestAborted { get => CancellationToken.None; set => throw new NotImplementedException(); }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Abort()
        {
        }
    }
}
