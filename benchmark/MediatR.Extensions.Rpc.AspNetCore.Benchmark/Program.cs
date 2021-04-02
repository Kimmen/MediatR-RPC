using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using MediatR.Rpc.AspNetCore.Benchmark.Middleware;
using MediatR.Rpc.AspNetCore.Benchmark.Requests;

using System;
using System.Linq;

namespace MediatR.Rpc.AspNetCore.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Number of available requests: {new RequestFactory().Count() }");

            var config = DefaultConfig.Instance
                .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                .CreateImmutableConfig();
            
            var dummySummary = BenchmarkRunner.Run<UsingDummyConfigurationsWithFakeRunner>(config);
            var defaultSummary = BenchmarkRunner.Run<UsingDefaultConfigurationsWithActualRunner>(config);

            var logger = ConsoleLogger.Default;
            MarkdownExporter.Console.ExportToLog(dummySummary, logger);
            ConclusionHelper.Print(logger, config.GetCompositeAnalyser().Analyse(dummySummary).ToList());

            MarkdownExporter.Console.ExportToLog(defaultSummary, logger);
            ConclusionHelper.Print(logger, config.GetCompositeAnalyser().Analyse(defaultSummary).ToList());

            Console.ReadKey();
        }
    }

    public class ClassNameColumn : IColumn
    {
        public ClassNameColumn()
        {

        }

        public string Id => "Class";

        public string ColumnName => "Class";

        public bool AlwaysShow => true;

        public ColumnCategory Category => ColumnCategory.Custom;

        public int PriorityInCategory => 0;

        public bool IsNumeric => false;

        public UnitType UnitType => UnitType.Dimensionless;

        public string Legend => $"Custom '{ColumnName}' tag column";

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
        {
            return benchmarkCase.Descriptor.Type.Name;
        }

        public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
        {
            return this.GetValue(summary, benchmarkCase);
        }

        public bool IsAvailable(Summary summary) => true;

        public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
    }
}
