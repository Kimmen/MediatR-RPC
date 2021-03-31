# MediatR-RPC

**MediatR-RPC** is an extension library for [MediatR](https://github.com/jbogard/MediatR "MediatR at Github") which aims automatically expose endpoints for corresponding [Requests](https://github.com/jbogard/MediatR/wiki#requestresponse) objects. Examples for an Http environment:
Request   | Http Endpoint
------    | ------
GetUser   | /api/getuser
WriteLoan | /api/writeloan
ApprovePost | /api/approvepost

## Configuration
MediatR-RPC in its core is about matching a name with a corresponding request type that can be constructed and process by MediatR. This core feature is then applied on different hosting applications. 

When configurating **MediatR-RPC** all configurations needs to be _explicitly_ set. This way it is more clear for the reader what is going and what the expected behaviors are. Altought there are no implicit defaults there are some _predefined configurations helpers_ available ready to use.

## Getting started in ASP.NET Core
In ASP.NET Core MediatR-RPC will configure itself as an EndpointMiddleware with a single templated route, example:
`https://localhost:44393/rpc/{requestName}`

Install package from Nuget: 
```
Install-Package MediatR.Extensions.RPC.AspNetCore
```

First register MediatR-RPC (and MediatR) when configurating the service by using the `.AddMediatrRpc(...)` extension method on the service collection:
```csharp
public void ConfigureServices(IServiceCollection services)
{
  // Additional configuration
  var targetAssembly = this.GetType().Assembly;
  services.AddMediatR(targetAssembly);
  services.AddMediatrRpc(o =>
  {
      o.ScanRequests(targetAssembly);
      o.UseExactRequestTypeNameMatchingConvention();
  });

  // Additional configuration
}
```

In ASP.NET Core MediatR-RPC configures an EndpointMiddleware by applying the `.MapRpc(...)` extension method on the endpoints configuration:
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  // Additional configuration
  app.UseEndpoints(endpoints =>
  {
      endpoints.MapControllers();
      endpoints.MapRpc(o =>
      {
          var jsonSerializationOptions = new System.Text.Json.JsonSerializerOptions
          {
              PropertyNameCaseInsensitive = true
          };

          o.Path = "rpc";
          o.UseSystemJsonForDeserializeBody(jsonSerializationOptions);
          o.UseSystemJsonForOkOrNotFoundResult(jsonSerializationOptions);
      });
  });
  // Additional configuration
}
```

## Getting started in Azure Functions

In Azure Function v3 MediatR-RPC will not be able to automatically add an Http triggered function, but it will instead provide a facade object for processing HttpRequests as Rpc calls. 

First create an Azure Function v3 project with a Http trigger function, for example [via Visual Studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio).

Install package from Nuget: 
```
Install-Package MediatR.Extensions.RPC.Azure.Functions
```

To be able to register MediatR-RPC the project needs to enable dependency injection by creating a [FunctionStartup](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection). 

Once created, register MediatR-RPC (and MediatR) when configurating the service by using the `.AddMediatrRpc(...)` and `.AddMediatrRpcHttp(...)` extension methods on the service collection:
```csharp
public void Configure(IWebJobsBuilder builder)
{
    var targetAssembly = this.GetType().Assembly;

    builder.Services
        .AddMediatR(targetAssembly)
        .AddMediatrRpc(o =>
        {
            o.ScanRequests(targetAssembly);
            o.UseExactRequestTypeNameMatchingConvention();
        })
        .AddMediatrRpcHttp(o => 
        {
            o.UseNewtonsoftJsonForDeserializeBody();
            o.UseOkorNotFoundActionResults();
        });
}
```

Configure the Http trigger function to be the RPC-endpoint by injecting the `IRpcHttpFunction` object and call it from the function:
```csharp
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
```