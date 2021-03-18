using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.Azure.Functions
{
    public interface IRpcHttpFunction
    {
        Task<IActionResult> ProcessHttpCall(string requestName, HttpRequest request, CancellationToken cancellationToken = default);
    }

    public class RpcHttpFunction : IRpcHttpFunction
    {
        private readonly RpcHttpFunctionOptions options;
        private readonly RpcCaller rpcCaller;

        public RpcHttpFunction(RpcHttpFunctionOptions options, RpcCaller rpcCaller)
        {
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
