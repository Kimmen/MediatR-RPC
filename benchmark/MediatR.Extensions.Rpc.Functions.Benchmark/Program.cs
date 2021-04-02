using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

using MediatR.Rpc.Functions.Benchmark.Requests;

using System;
using System.Linq;

namespace MediatR.Rpc.Functions.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Number of available requests: {new RequestFactory().Count() }");

            var config = DefaultConfig.Instance
                .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                .CreateImmutableConfig();

            var dummySummary = BenchmarkRunner.Run<UsingDummyConfigurationWithFakeRunner>(config);
            var defaultSummary = BenchmarkRunner.Run<UsingDefaultConfigurationWithActualRunner>(config);

            var logger = ConsoleLogger.Default;
            MarkdownExporter.Console.ExportToLog(dummySummary, logger);
            ConclusionHelper.Print(logger, config.GetCompositeAnalyser().Analyse(dummySummary).ToList());

            MarkdownExporter.Console.ExportToLog(defaultSummary, logger);
            ConclusionHelper.Print(logger, config.GetCompositeAnalyser().Analyse(defaultSummary).ToList());

            Console.ReadKey();
        }
    }
}
