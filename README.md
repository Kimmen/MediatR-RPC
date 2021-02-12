# MediatR-RPC
Automatically expose your MediatR requests as Remote Procedure Call (RPC) like endpoints.

## Introduction
[MediatR](https://github.com/jbogard/MediatR "MediatR at Github") is a simple, unambitious mediator implementation in .NET made by [Jimmy Bogard](https://twitter.com/jbogard?s=20 "Jimmy Bogard at Twitter"). **MediatR-RPC** extension library aims to automatically expose endpoint to process the [Requests](https://github.com/jbogard/MediatR/wiki#requestresponse) using configuration. In ASP.NET Core a single templated route is exposed where clients can POST requests.

When configurating **MediatR-RPC** all configurations needs to be _explicitly_ set. This way it is more clear for the reader what is going and what the expected behaviors are. Altought there are no implicit defaults there are some _predefined configurations helpers_ available ready to use.

## Getting started in ASP.NET Core
In ASP.NET Core MediatR-RPC will configure itself as an EndpointMiddleware whith available configurations. This example will configure a route that a client can POST request and the body will be serialized as JSON: 
```curl
curl -X POST \
> https://localhost:44393/rpc/helloworldrequest \
> -H 'Content-Type:application/json' \
> -d '{ "name":"Joakim" }'
```

Install package from Nuget: 
`TBD`

[//]: # (`Install-Package MediatR`)

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

