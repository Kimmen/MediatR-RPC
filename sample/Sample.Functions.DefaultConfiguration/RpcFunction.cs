using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Threading;

namespace Sample.Functions.DefaultConfiguration
{
    public class RpcFunction
    {
        private readonly ISender sender;

        public RpcFunction(ISender sender) 
        {
            this.sender = sender;
        }

        [FunctionName("HelloWorld")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log, 
            CancellationToken cancellationToken)
        {
            var response = await this.sender.Send(new Handlers.HelloWorldRequest { Name = "Default name" }, cancellationToken);

            return new OkObjectResult(response);
        }
    }
}
