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
        Task<IActionResult> ProcessHttpCall(string requestName, HttpRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Process the request for Http trigger as RPC requests.
    /// </summary>
    public class RpcHttpFunction : IRpcHttpFunction
    {
        private readonly RpcHttpFunctionOptions options;
        private readonly RpcCaller rpcCaller;

        public RpcHttpFunction(RpcHttpFunctionOptions options, RpcCaller rpcCaller)
        {
            RpcHttpFunctionValidator.ValidateCaller(rpcCaller);
            RpcHttpFunctionValidator.ValidateOptions(options);

            this.options = options;
            this.rpcCaller = rpcCaller;
        }

        public async Task<IActionResult> ProcessHttpCall(string requestName, HttpRequest request, CancellationToken cancellationToken = default)
        {
            var result = await this.rpcCaller.Process(requestName, (t, ct) => options.DeserializeRequest((t, request), ct), cancellationToken);

            return await this.options.HandleResponse((result, request), cancellationToken);
        }
    }
}
