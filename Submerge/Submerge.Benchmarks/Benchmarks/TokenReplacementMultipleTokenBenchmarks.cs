using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using StringTokenFormatter;
using Submerge.Configuration;

namespace Submerge.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class TokenReplacementMultipleTokenBenchmarks
    {
        private class TestClass
        {
            public string Name { get; init; }
            public string Location { get; init; }
            public string State { get; init; }
            public string Country { get; init; }
            public int Age { get; init; }
            public int Rank { get; init; }
            
        }

        private const string _testString = Constants.ExampleMergeTemplate;
        private const int _age = 99;
        private const int _rank = 1;
        private const string _name = "John";
        private const string _location = "Melbourne";
        private const string _state = "Victoria";
        private const string _country = "Australia";

        [Benchmark(Baseline = true)]
        public void StringTokenFormatter()
        {
            var tokenValues = new TestClass() { 
                Name = _name,
                Location = _location,
                State = _state,
                Country = _country,
                Age = _age,
                Rank = _rank
            };
            
            _testString.FormatToken(tokenValues);
        }

        [Benchmark]
        public void EscapeRouteTokenReplacementEngine()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("name", _name)
                .AddMapping("age", _age.ToString())
                .AddMapping("rank", _rank.ToString())
                .AddMapping("location", _location)
                .AddMapping("state", _state)
                .AddMapping("country", _country)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            submergeTokenReplacer.Replace(_testString);
        }

        [Benchmark]
        public void NativeStringReplace()
        {
            var result = _testString.Replace("{name}", _name)
                .Replace("{age}", _age.ToString())
                .Replace("{rank}", _rank.ToString())
                .Replace("{location}", _location)
                .Replace("{state}", _state)
                .Replace("{country}", _country);

        }
    }
}