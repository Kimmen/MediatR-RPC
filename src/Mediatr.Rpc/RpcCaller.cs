using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace Mediatr.Rpc
{
    public class RpcCallExecuter
    {
        private readonly ISender sender;
        private readonly LinearSearchRequestTypeProvider requestTypeProvider;

        public RpcCallExecuter(ISender sender, RpcOptions options)
        {
            this.sender = sender;
            this.requestTypeProvider = new LinearSearchRequestTypeProvider(options.Requests, options.MatchingConvention);
        }

        public async Task<Result> Process(string requestName, Func<Type, CancellationToken, Task<object>> contentDeserializer, CancellationToken cancellationToken)
        {
            if (false == this.requestTypeProvider.TryGetByName(requestName, out var requestType))
            {
                return Result.NotFound();
            }

            var request = await contentDeserializer(requestType, cancellationToken);
            var response = await this.sender.Send(request, cancellationToken);

            return Result.Ok(response);
        }

        public struct Result
        {
            public bool FoundHandler { get; internal set; }
            public object Response { get; internal set; }

            internal static Result Ok(object response)
            {
                return new Result
                {
                    FoundHandler = true,
                    Response = response
                };
            }

            internal static Result NotFound()
            {
                return new Result
                {
                    FoundHandler = false,
                    Response = null
                };
            }
        }
    }
}