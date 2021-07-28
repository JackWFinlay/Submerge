using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using StringTokenFormatter;
using Submerge.Configuration;

namespace Submerge.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class TokenReplacementLoopBenchmarks
    {
        private class TestClass
        {
            public string Name { get; set; }
            public string Location { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public int Age { get; set; }
            public int Rank { get; set; }
            
        }
        
        private const int _iterations = 10_000;
        private const int _age = 99;
        private const string _testString = Constants.ExampleMergeTemplate;
        private const string _name = "John";
        private const string _location = "Melbourne";
        private const string _state = "Victoria";
        private const string _country = "Australia";

        private readonly TestClass[] _testClasses;

        public TokenReplacementLoopBenchmarks()
        {
            _testClasses = new TestClass[_iterations];
            for (var i = 0; i < _iterations; i++)
            {
                _testClasses[i] = new TestClass()
                {
                    Name = _name,
                    Location = _location,
                    State = _state,
                    Country = _country,
                    Age = _age,
                    Rank = i
                };
            }
        }

        [Benchmark(Baseline = true)]
        public void StringTokenFormatter()
        {
            for (var i = 0; i < _iterations; i++)
            {
                _testString.FormatToken(_testClasses[i]);
            }
        }

        [Benchmark]
        public async Task SubmergeTokenReplacementEngineFromObject()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var testMemory = _testString.AsMemory();

            for (var i = 0; i < _iterations; i++)
            {
                config.UpdateOrAddFromObject(_testClasses[i]);
                
                await submergeTokenReplacer.ReplaceAsync(testMemory);
            }
        }
        
        [Benchmark]
        public async Task SubmergeTokenReplacementEngineUpdateOrAddMapping()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var testMemory = _testString.AsMemory();
            
            for (var i = 0; i < _iterations; i++)
            {
                config.UpdateOrAddMapping("name", _testClasses[i].Name);
                config.UpdateOrAddMapping("age", _testClasses[i].Age.ToString());
                config.UpdateOrAddMapping("rank", _testClasses[i].Rank.ToString());
                config.UpdateOrAddMapping("location", _testClasses[i].Location);
                config.UpdateOrAddMapping("state", _testClasses[i].State);
                config.UpdateOrAddMapping("country", _testClasses[i].Country);

                await submergeTokenReplacer.ReplaceAsync(testMemory);
            }
        }
        
        [Benchmark]
        public void SubmergeTokenReplacementEngineReplaceEnumerable()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var testMemory = _testString.AsMemory();
            var subMaps = new List<IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>>();
            
            for (var i = 0; i < _iterations; i++)
            {
                var subConfig = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                    .SetTokenEnd("}")
                    .AddMapping("name", _testClasses[i].Name)
                    .AddMapping("age", _testClasses[i].Age.ToString())
                    .AddMapping("rank", _testClasses[i].Rank.ToString())
                    .AddMapping("location", _testClasses[i].Location)
                    .AddMapping("state", _testClasses[i].State)
                    .AddMapping("country", _testClasses[i].Country);
                
                subMaps.Add(subConfig.SubstitutionMap);
            }

            foreach (var result in submergeTokenReplacer.ReplaceAsync(testMemory, subMaps))
            {
            }
        }

        [Benchmark]
        public void NativeStringReplace()
        {
            for (var i = 0; i < _iterations; i++)
            {
                var result = _testString.Replace("{name}", _name)
                    .Replace("{age}", _age.ToString())
                    .Replace("{rank}", i.ToString())
                    .Replace("{location}", _location)
                    .Replace("{state}", _state)
                    .Replace("{country}", _country);
            }
        }
    }
}