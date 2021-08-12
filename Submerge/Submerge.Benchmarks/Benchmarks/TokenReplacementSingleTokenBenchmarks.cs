using BenchmarkDotNet.Attributes;
using StringTokenFormatter;
using Submerge.Abstractions.Models;
using Submerge.Configuration;

namespace Submerge.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class TokenReplacementSingleTokenBenchmarks
    {
        private const string _testString = Constants.ExampleMergeTemplate;
        private const string _tokenValue = "name";
        private const string _name = "John";

        [Benchmark(Baseline = true)]
        public void StringTokenFormatter()
        {
            _testString.FormatToken(_tokenValue, _name);
        }

        [Benchmark]
        public void SubmergeReplace()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping(_tokenValue, _name)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            submergeTokenReplacer.Replace(_testString);
        }
        
        [Benchmark]
        public void SubmergeReplaceFixedFormat()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var matchSet = submergeTokenReplacer.GetFixedMatches(_testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(_name);
                
            submergeTokenReplacer.Replace(matchSet, tokenReplacementSet);
        }
        
        [Benchmark]
        public void StringReplace()
        {
            var result = _testString.Replace("{name}", _name);
        }
        
        [Benchmark]
        public void StringInterpolation()
        {
            var result = $"Name:{_name} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean luctus viverra ante, ac tempus diam efficitur feugiat. Age:{{age}} Praesent ipsum est, convallis at vehicula venenatis, convallis vitae nulla. Morbi tempus sed sapien nec sodales. Rank:{{rank}} Donec tellus nunc, fringilla eu dolor sit amet, eleifend efficitur sem. Vestibulum non diam tortor. Location:{{location}} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam quis mattis mauris. Morbi ac ultrices dolor State:{{state}} Duis id consectetur lacus. Fusce ac neque at odio interdum commodo. Quisque ac magna metus. Maecenas sed lacinia odio Country:{{country}} Aenean at odio volutpat, molestie felis id, vestibulum tellus. Vivamus pellentesque ut urna iaculis vehicula.";
        }
        
        [Benchmark]
        public void StringFormat()
        {
            var result = string.Format("Name:{0} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean luctus viverra ante, ac tempus diam efficitur feugiat. Age:{{age}} Praesent ipsum est, convallis at vehicula venenatis, convallis vitae nulla. Morbi tempus sed sapien nec sodales. Rank:{{rank}} Donec tellus nunc, fringilla eu dolor sit amet, eleifend efficitur sem. Vestibulum non diam tortor. Location:{{location}} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam quis mattis mauris. Morbi ac ultrices dolor State:{{state}} Duis id consectetur lacus. Fusce ac neque at odio interdum commodo. Quisque ac magna metus. Maecenas sed lacinia odio Country:{{country}} Aenean at odio volutpat, molestie felis id, vestibulum tellus. Vivamus pellentesque ut urna iaculis vehicula.",
                _name);
        }
    }
}