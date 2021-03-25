using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Azure.Functions
{
    /// <summary>
    /// Abstraction for the RPC functionality for Http triggers.
    /// </summary>
    public interface IRpcHttpFunction
    {
        /// <summary>
        /// Process the Http request as an Rpc request.
        /// </summary>
        /// <param name="requestName">The name of the Rpc request to process.</param>
        /// <param name="request">The Http request.</param>
        /// <param name="cancellationToken">CancellationToken for the request scope.</param>
        /// <returns>The Http action result that corresponds with the Rpc result.</returns>
        Task<IActionResult> ProcessHttpRequest(string requestName, HttpRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Process the request for Http trigger as RPC requests.
    /// </summary>
    public class RpcHttpFunction : IRpcHttpFunction
    {
        private readonly RpcHttpFunctionOptions options;
        private readonly IRpcRequestRunner rpcCaller;

        public RpcHttpFunction(RpcHttpFunctionOptions options, IRpcRequestRunner rpcCaller)
        {
            RpcHttpFunctionValidator.ValidateCaller(rpcCaller);
            RpcHttpFunctionValidator.ValidateOptions(options);

            this.options = options;
            this.rpcCaller = rpcCaller;
        }

        /// <summary>
        /// Process the Http request as an Rpc request.
        /// </summary>
        /// <param name="requestName">The name of the Rpc request to process.</param>
        /// <param name="request">The Http request.</param>
        /// <param name="cancellationToken">CancellationToken for the request scope.</param>
        /// <returns>The Http action result that corresponds with the Rpc result.</returns>
        public async Task<IActionResult> ProcessHttpRequest(string requestName, HttpRequest request, CancellationToken cancellationToken = default)
        {
            var result = await this.rpcCaller.Process(requestName, (t, ct) => options.DeserializeRequest((t, request), ct), cancellationToken);

            return await this.options.HandleResponse((result, request), cancellationToken);
        }
    }
}
