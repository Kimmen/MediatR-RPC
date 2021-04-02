using FakeItEasy;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Core.Tests.RpcRequestRunner
{
    internal class RpcRunnerFixture
    {
        private readonly ISender senderFake;
        private readonly RpcOptions options;
        private Func<Type, CancellationToken, Task<object>> requestValueFactory;
        private string requestName;

        public RpcRunnerFixture()
        {
            this.senderFake = A.Fake<ISender>();
            this.options = new RpcOptions()
                .UseExactRequestTypeNameMatchingConvention();
        }

        internal RpcRunnerFixture WithRegistratedRequestTypes(params Type[] types)
        {
            options.Requests = types;
            return this;
        }

        internal RpcRunnerFixture WithExactNameConvention()
        {
            options.UseExactRequestTypeNameMatchingConvention();
            return this;
        }
        internal RpcRunnerFixture WithResponse(object response)
        {
            A.CallTo(() => this.senderFake.Send(A<object>.Ignored, A<CancellationToken>.Ignored))
                .ReturnsLazily(() => Task.FromResult(response));

            return this;
        }

        internal RpcRunnerFixture WithRequest(object request)
        {
            this.requestName = request.GetType().Name;
            this.requestValueFactory = (t, ct) => Task.FromResult(request);

            return this;
        }

        internal async Task<IRpcResult> Process()
        {
            var runner  = new Rpc.RpcRequestRunner(this.senderFake, this.options);

            return await runner.Process(this.requestName, this.requestValueFactory);
        }        
    }
}
