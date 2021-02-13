# MediatR-RPC
Automatically expose your MediatR requests as Remote Procedure Call (RPC) like endpoints.

## Introduction
[MediatR](https://github.com/jbogard/MediatR "MediatR at Github") is a simple, unambitious mediator implementation in .NET made by [Jimmy Bogard](https://twitter.com/jbogard?s=20 "Jimmy Bogard at Twitter"). **MediatR-RPC** is an extension library aims to automatically expose endpoints to process the [Requests](https://github.com/jbogard/MediatR/wiki#requestresponse) using configuration. 

When configurating **MediatR-RPC** all configurations needs to be _explicitly_ set. This way it is more clear for the reader what is going and what the expected behaviors are. Altought there are no implicit defaults there are some _predefined configurations helpers_ available ready to use.

## Getting started in ASP.NET Core
In ASP.NET Core MediatR-RPC will configure itself as an EndpointMiddleware with a single templated route that only allows POST method, example:
`https://localhost:44393/rpc/{requestName}`

Install package from Nuget: 
```
Install-Package MediatR.Extensions.RPC.AspNetCore
```

First register MediatR-RPC when configurating the service by using the `.AddMediatrRpc(...)` extension method on the service collection:
```csharp
public void ConfigureServices(IServiceCollection services)
{
  // Additional configuration
  var targetAssembly = this.GetType().Assembly;
  services.AddMediatR(targetAssembly);
  services.AddMediatrRpc(o =>
  {
      o.ScanRequests(targetAssembly);
      o.UseRequestNameMatchingConvention();
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
          o.Path = "rpc";
          o.SerializeWithSystemJson(new System.Text.Json.JsonSerializerOptions
          {
              PropertyNameCaseInsensitive = true
          });
          o.ResponsesAs200Ok();
          o.UnmatchedRequestsAs404NotFound();
      });
  });
  // Additional configuration
}
```

## Configuration
MediatR-RPC in its core is about matching a name with a corresponding request type that can be constructed and process by MediatR. This core feature is then applied on different hosting applications. In ASP.NET Core an EndpointMiddleware is configured so a name and properties can be extracted from HTTP request messages.

There are two parts of configurating MediatR-RCP. First configure which requests that are available and how they are mapped. This is done in `.AddMediatrRpc(...)` extenstion method and applying the `RpcOptions`. Second, depending on the host application, the endpoints are configured. 

### RpcOptions
Property                          | Description
------                            | ------
Requests                          | A collection of MediatR requests that are available for matching.
MatchingConvention                | Given a request which matching name should be derived from it.

Helping methods                   | Description
------                            | ------
ScanRequests()                    | Scans MediatR requests in the specified assmeblies.
UseRequestNameMatchingConvention()| The request class name is used when matching types.

### RpcEndpointOptions in ASP.NET Core
Property                          | Description
------                            | ------
Path                              | The URL root path for the endpoint.
DeserializeRequest                | Deserializes the HTTP request body.
SerializeResponse                 | Seserializes the MediatR response to the HTTP response body.
UnmatchedRequest                  | Handler for unmatched requests.
HandlResponse                     | Handler for matched requests.

Helping methods                   | Description
------                            | ------
SerializeWithSystemJson()         | Serialize and Deserialize HTTP body as JSON.
UnmatchedRequestsAs404NotFound()  | Unmatched requests are returned as 404 - NotFound HTTP responses.
ResponsesAs200Ok()                | Matched requests are returned as 200 - OK HTTP responses.
