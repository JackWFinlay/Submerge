using BenchmarkDotNet.Running;
using Submerge.Benchmarks.Benchmarks;

namespace Submerge.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // BenchmarkRunner.Run(typeof(TokenReplacementMultipleTokenBenchmarks));
            // BenchmarkRunner.Run(typeof(TokenReplacementSingleTokenBenchmarks));
            BenchmarkRunner.Run(typeof(TokenReplacementLoopBenchmarks));
        }
    }
}