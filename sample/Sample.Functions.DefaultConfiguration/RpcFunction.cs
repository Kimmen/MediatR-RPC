using MediatR.Rpc.Azure.Functions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace Sample.Functions.DefaultConfiguration
{
    public class RpcFunction
    {
        private readonly IRpcHttpFunction rpcFunction;
        private readonly ILogger<RpcFunction> logger;

        public RpcFunction(IRpcHttpFunction rpcFunction, ILogger<RpcFunction> logger) 
        {
            this.rpcFunction = rpcFunction;
            this.logger = logger;
        }

        [FunctionName("rpc")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{requestName}")] HttpRequest req,
            string requestName,
            CancellationToken cancellationToken)
        {
            this.logger.LogDebug("Processing request: {RequestName}", requestName);
            return await this.rpcFunction.ProcessHttpRequest(requestName, req, cancellationToken);
        }
    }
}
