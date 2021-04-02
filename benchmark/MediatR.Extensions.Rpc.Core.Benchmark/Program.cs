using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using MediatR.Rpc.Benchmark.Requests;

using System;

namespace MediatR.Rpc.Core.Benchmark
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine($"Number of available requests: {new RequestFactory().Count() }");

            var config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
            BenchmarkRunner.Run<UsingDefaultConfigurationWithDummyValueFactory>(config);

            Console.ReadKey();
        }
    }
}
