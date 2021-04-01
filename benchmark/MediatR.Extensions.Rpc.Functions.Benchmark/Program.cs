using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using MediatR.Rpc.Functions.Benchmark.Requests;

using System;

namespace MediatR.Rpc.Functions.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Number of available requests: {new RequestFactory().Count() }");

            var config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
            BenchmarkRunner.Run<SlimHttpFunctionProcessRequest>(config);

            Console.ReadKey();
        }
    }
}
