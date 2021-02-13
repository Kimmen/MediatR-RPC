using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace Mediatr.Rpc
{
    /// <summary>
    /// Finds corresponding requests and process them. 
    /// </summary>
    public class RpcCaller
    {
        private readonly ISender sender;
        private readonly LinearSearchRequestTypeProvider requestTypeProvider;

        public RpcCaller(ISender sender, RpcOptions options)
        {
            RcpCallerValidator.ValidateSender(sender);
            RcpCallerValidator.ValidateOptions(options);

            this.sender = sender;
            this.requestTypeProvider = new LinearSearchRequestTypeProvider(options.Requests, options.MatchingConvention);
        }

        /// <summary>
        /// Finds, instantiates and process requests that corresponds to the specified name.
        /// </summary>
        /// <param name="requestName">The name of the requests to process</param>
        /// <param name="requestDeserializer">Deserializer the requests.</param>
        /// <param name="cancellationToken">Optional Cancellation token.</param>
        /// <returns>Result indicating how the request was processed, containing the response if successful.</returns>
        public async Task<Result> Process(string requestName, Func<Type, CancellationToken, Task<object>> requestDeserializer, CancellationToken cancellationToken = default)
        {
            if (false == this.requestTypeProvider.TryGetByName(requestName, out var matchedRequestType))
            {
                return Result.NotFound();
            }

            var request = await requestDeserializer(matchedRequestType, cancellationToken);
            var response = await this.sender.Send(request, cancellationToken);

            return Result.Ok(response);
        }

        /// <summary>
        /// Result of request process.
        /// </summary>
        public struct Result
        {
            /// <summary>
            /// True if there was a corresponding request; otherwise false.
            /// </summary>
            public bool RequestFound { get; internal set; }
            /// <summary>
            /// The response of request process.
            /// </summary>
            public object? Response { get; internal set; }

            internal static Result Ok(object? response)
            {
                return new Result
                {
                    RequestFound = true,
                    Response = response
                };
            }

            internal static Result NotFound()
            {
                return new Result
                {
                    RequestFound = false,
                    Response = null
                };
            }
        }
    }
}