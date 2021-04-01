﻿using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Benchmark
{
    public class FakeSender : ISender
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
#nullable disable
            var response = default(TResponse);
            return Task.FromResult(response);
#nullable restore
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            object? response = null;
            return Task.FromResult(response);
        }
    }
}
