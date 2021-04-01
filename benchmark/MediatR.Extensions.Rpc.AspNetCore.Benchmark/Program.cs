using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using MediatR.Rpc.AspNetCore.Benchmark.Requests;

using System;
using System.Threading.Tasks;

namespace MediatR.Rpc.AspNetCore.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Number of available requests: {new RequestFactory().Count() }");

            var config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
            BenchmarkRunner.Run<MiddlewareProcessRequest>(config);

            Console.ReadKey();
        }
    }
}
