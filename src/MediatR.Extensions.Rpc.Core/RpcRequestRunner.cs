
using MediatR.Rpc.Validation;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc
{
    /// <summary>
    /// Finds corresponding requests and process them. 
    /// </summary>
    public interface IRpcRequestRunner
    {
        Task<IRpcResult> Process(string requestName, Func<Type, CancellationToken, Task<object>> requestValueFactory, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Finds corresponding requests and process them. 
    /// </summary>
    public sealed class RpcRequestRunner : IRpcRequestRunner
    {
        private readonly ISender sender;
        private readonly LinearSearchRequestTypeProvider requestTypeProvider;

        public RpcRequestRunner(ISender sender, RpcOptions options)
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
        /// <param name="requestValueFactory">Creates request objects given a Type.</param>
        /// <param name="cancellationToken">Optional Cancellation token.</param>
        /// <returns>Result indicating how the request was processed, containing the response if successful.</returns>
        public async Task<IRpcResult> Process(string requestName, Func<Type, CancellationToken, Task<object>> requestValueFactory, CancellationToken cancellationToken = default)
        {
            if (false == this.requestTypeProvider.TryGetByName(requestName, out var matchedRequestType))
            {
                return ResultAs.NotFound(requestName);
            }

            var request = await requestValueFactory(matchedRequestType, cancellationToken);
            var response = await this.sender.Send(request, cancellationToken);

            return ResultAs.Ok(response);
        }

        internal static class ResultAs
        {
            internal static SuccessfullyProcessedRequestResult Ok(object? response)
            {
                return new SuccessfullyProcessedRequestResult
                {
                    Response = response
                };
            }

            internal static NotFoundRequestResult NotFound(string requestName)
            {
                return new NotFoundRequestResult
                {
                    RequestName = requestName
                };
            }
        }
    }
}