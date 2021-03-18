
using MediatR.Rpc;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Threading.Tasks;

namespace MediatR.Rpc.Azure.Functions
{
    public static class RpcHttpFunctionOptionsConfigurator
    {
        public static RpcHttpFunctionOptions DeserializeWithNewtonsoftJson(this RpcHttpFunctionOptions options, JsonSerializerSettings? jsonSettings = null)
        {
            options.DeserializeRequest = async (input, ct) =>
            {
                var (targetType, httpRequest) = input;
;
                var hasContent = httpRequest.ContentLength > 0;

                if (hasContent)
                {
                    try
                    {
                        using var reader = new StreamReader(httpRequest.Body);
                        var bodyValue = await reader.ReadToEndAsync();

                        ct.ThrowIfCancellationRequested();

                        return JsonConvert.DeserializeObject(bodyValue, targetType, jsonSettings);
                    }
                    catch (JsonException ex)
                    {
                        throw new ArgumentException("Failed to deserialize body as JSON", ex);
                    }
                }

                //If there is no body, let's create is using default constructor. 
                return Activator.CreateInstance(targetType) ?? throw new ArgumentException($"Failed to construct instance of '{targetType.FullName}' using default constructor.", targetType.Name);
            };

            return options;
        }

        public static RpcHttpFunctionOptions RpcResultAsOkOrNotFound(this RpcHttpFunctionOptions options)
        {
            options.HandleResponse = (input, ct) =>
            {
                var (result, httpRequest) = input;

                IActionResult actionResult = result switch
                {
                    SuccessfullyProcessedRequestResult r => new OkObjectResult(r.Response),
                    NotFoundRequestResult r => new NotFoundObjectResult($"{r.RequestName} not found"),
                    _ => throw new NotImplementedException()
                };

                return Task.FromResult(actionResult);
            };

            return options;
        }
    }
}


