# MediatR-RPC
Automatically expose your MediatR requests as Remote Procedure Call (RPC) like endpoints.

## Introduction
[MediatR](https://github.com/jbogard/MediatR "MediatR at Github") is a simple, unambitious mediator implementation in .NET made by [Jimmy Bogard](https://twitter.com/jbogard?s=20 "Jimmy Bogard at Twitter"). This extension library aims to automatically expose the [Requests](https://github.com/jbogard/MediatR/wiki#requestresponse) as endpoints in ASP.NET Core with configuration without manually create an API Controller and call the MediatR.

## Getting started in ASP.NET Core
In ASP.NET Core MediatR-RPC will configure itself as a EndpointMiddleware whith available configurations.
Install package from Nuget: 
`TBD`

[//]: # (`Install-Package MediatR`)

First register MediatR-RPC when configurating the service:
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


###  Design strategy
When configurating **MediatR-RPC** all configurations needs to be _explicitly_ set by the user. This way it should be clear for the user and the successors what is going and what the expected behaviors are. Altought there are no hidden defaults there are some _predefined configurations_ available ready to use. 
