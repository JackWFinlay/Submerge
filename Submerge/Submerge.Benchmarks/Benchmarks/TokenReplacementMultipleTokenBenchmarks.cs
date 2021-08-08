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
        public void SubmergeMapFromConfig()
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
        public void SubmergeMapFromSubMap()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
                
            var subMap = new SubstitutionMap()
                .UpdateOrAddMapping("name", _name)
                .UpdateOrAddMapping("age", _age.ToString())
                .UpdateOrAddMapping("rank", _rank.ToString())
                .UpdateOrAddMapping("location", _location)
                .UpdateOrAddMapping("state", _state)
                .UpdateOrAddMapping("country", _country);

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);

            var pattern = submergeTokenReplacer.GetMatches(_testString);
            
            submergeTokenReplacer.Replace(pattern, subMap);
        }

        [Benchmark]
        public void StringReplace()
        {
            var result = _testString.Replace("{name}", _name)
                .Replace("{age}", _age.ToString())
                .Replace("{rank}", _rank.ToString())
                .Replace("{location}", _location)
                .Replace("{state}", _state)
                .Replace("{country}", _country);
        }
        
        [Benchmark]
        public void StringInterpolation()
        {
            var result =
                $"Name:{_name} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean luctus viverra ante, ac tempus diam efficitur feugiat. Age:{_age.ToString()} Praesent ipsum est, convallis at vehicula venenatis, convallis vitae nulla. Morbi tempus sed sapien nec sodales. Rank:{_rank.ToString()} Donec tellus nunc, fringilla eu dolor sit amet, eleifend efficitur sem. Vestibulum non diam tortor. Location:{_location} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam quis mattis mauris. Morbi ac ultrices dolor State:{_state} Duis id consectetur lacus. Fusce ac neque at odio interdum commodo. Quisque ac magna metus. Maecenas sed lacinia odio Country:{_country} Aenean at odio volutpat, molestie felis id, vestibulum tellus. Vivamus pellentesque ut urna iaculis vehicula.";
        }
        
        [Benchmark]
        public void StringFormat()
        {
            var result = 
                string.Format("Name:{0} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean luctus viverra ante, ac tempus diam efficitur feugiat. Age:{1} Praesent ipsum est, convallis at vehicula venenatis, convallis vitae nulla. Morbi tempus sed sapien nec sodales. Rank:{2} Donec tellus nunc, fringilla eu dolor sit amet, eleifend efficitur sem. Vestibulum non diam tortor. Location:{3} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam quis mattis mauris. Morbi ac ultrices dolor State:{4} Duis id consectetur lacus. Fusce ac neque at odio interdum commodo. Quisque ac magna metus. Maecenas sed lacinia odio Country:{5} Aenean at odio volutpat, molestie felis id, vestibulum tellus. Vivamus pellentesque ut urna iaculis vehicula.",
                              _name, _age.ToString(), _rank.ToString(), _location, _state, _country);
        }
    }
}