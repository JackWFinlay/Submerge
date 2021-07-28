using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using StringTokenFormatter;
using Submerge.Configuration;
using Submerge.ReplacementEngines;

namespace Submerge.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class TokenReplacementSingleTokenBenchmarks
    {
        private class TestClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        
        private const string _testString = Constants.ExampleMergeTemplate;
        private const string _tokenValue = "name";
        private const string _name = "John";

        [Benchmark(Baseline = true)]
        public void StringTokenFormatter()
        {
            _testString.FormatToken(_tokenValue, _name);
        }

        [Benchmark]
        public async Task EscapeRouteTokenReplacementEngine()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping(_tokenValue, _name)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            await submergeTokenReplacer.ReplaceAsync(_testString.AsMemory());
        }
        
        [Benchmark]
        public void NativeStringReplace()
        {
            var result = _testString.Replace("{name}", _name);
        }
    }
}