using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using StringTokenFormatter;
using Submerge.Abstractions.Interfaces;
using Submerge.Abstractions.Models;
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
            public string Age { get; set; }
            public string Rank { get; set; }
            
        }
        
        private const int _iterations = 10_000;
        private const string _age = "99";
        private const string _testString = Constants.ExampleMergeTemplate;
        private const string _name = "John";
        private const string _location = "Melbourne";
        private const string _state = "Victoria";
        private const string _country = "Australia";

        private readonly TestClass[] _testClasses;
        private readonly ISubstitutionMap[] _substitutionMaps;
        private readonly ITokenValueContainer[] _tokenValueContainers;
        private readonly TokenReplacementSet[] _tokenReplacementSets;

        private readonly SubmergeTokenReplacer _submergeTokenReplacer;
        private readonly FixedTokenMatchSet _fixedTokenMatchSet;

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
                    Rank = i.ToString()
                };
            }

            _substitutionMaps = new ISubstitutionMap[_iterations];
            for (var i = 0; i < _iterations; i++)
            {
                _substitutionMaps[i] = new SubstitutionMap()
                    .UpdateOrAddMapping("name", _testClasses[i].Name)
                    .UpdateOrAddMapping("age", _testClasses[i].Age)
                    .UpdateOrAddMapping("rank", _testClasses[i].Rank)
                    .UpdateOrAddMapping("location", _testClasses[i].Location)
                    .UpdateOrAddMapping("state", _testClasses[i].State)
                    .UpdateOrAddMapping("country", _testClasses[i].Country);
            }

            _tokenValueContainers = new ITokenValueContainer[_iterations];
            for (var i = 0; i < _iterations; i++)
            {
                _tokenValueContainers[i] = TokenValueContainer.FromObject(_testClasses[i]);
            }

            _tokenReplacementSets = new TokenReplacementSet[_iterations];
            for (var i = 0; i < _iterations; i++)
            {
                _tokenReplacementSets[i] = new TokenReplacementSet()
                    .AddReplacement(_testClasses[i].Name)
                    .AddReplacement(_testClasses[i].Age)
                    .AddReplacement(_testClasses[i].Rank)
                    .AddReplacement(_testClasses[i].Location)
                    .AddReplacement(_testClasses[i].State)
                    .AddReplacement(_testClasses[i].Country);
            }
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            _submergeTokenReplacer = new SubmergeTokenReplacer(config);
            _fixedTokenMatchSet = _submergeTokenReplacer.GetFixedMatches(_testString);
        }

        [Benchmark]
        public void StringTokenFormatter()
        {
            for (var i = 0; i < _iterations; i++)
            {
                _testString.FormatToken(_testClasses[i]);
            }
        }
        
        [Benchmark]
        public void StringTokenFormatter_FormatContainer()
        {
            var pattern = SegmentedString.Parse(_testString);
        
            for (var i = 0; i < _iterations; i++)
            {
                var propertiesContainer = TokenValueContainer.FromObject(_testClasses[i]);
        
                pattern.Format(propertiesContainer);
            }
        }
        
        [Benchmark]
        public void StringTokenFormatter_FormatContainerPreCalculatedContainer()
        {
            var pattern = SegmentedString.Parse(_testString);
        
            for (var i = 0; i < _iterations; i++)
            {
                pattern.Format(_tokenValueContainers[i]);
            }
        }
        
        [Benchmark]
        public void StringReplace()
        {
            for (var i = 0; i < _iterations; i++)
            {
                var result = _testString.Replace("{name}", _testClasses[i].Name)
                    .Replace("{age}", _testClasses[i].Age)
                    .Replace("{rank}", _testClasses[i].Rank)
                    .Replace("{location}", _testClasses[i].Location)
                    .Replace("{state}", _testClasses[i].State)
                    .Replace("{country}", _testClasses[i].Country);
            }
        }
        
        [Benchmark(Baseline = true)]
        public void StringFormat()
        {
            for (var i = 0; i < _iterations; i++)
            {
                var result = string.Format("Name:{0} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean luctus viverra ante, ac tempus diam efficitur feugiat. Age:{1} Praesent ipsum est, convallis at vehicula venenatis, convallis vitae nulla. Morbi tempus sed sapien nec sodales. Rank:{2} Donec tellus nunc, fringilla eu dolor sit amet, eleifend efficitur sem. Vestibulum non diam tortor. Location:{3} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam quis mattis mauris. Morbi ac ultrices dolor State:{4} Duis id consectetur lacus. Fusce ac neque at odio interdum commodo. Quisque ac magna metus. Maecenas sed lacinia odio Country:{5} Aenean at odio volutpat, molestie felis id, vestibulum tellus. Vivamus pellentesque ut urna iaculis vehicula.",
                    _testClasses[i].Name,
                    _testClasses[i].Age,
                    _testClasses[i].Rank,
                    _testClasses[i].Location,
                    _testClasses[i].State,
                    _testClasses[i].Country);
            }
        }
        
        [Benchmark]
        public void StringInterpolation()
        {
            for (var i = 0; i < _iterations; i++)
            {
                var result = $"Name:{_testClasses[i].Name} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean luctus viverra ante, ac tempus diam efficitur feugiat. Age:{_testClasses[i].Age} Praesent ipsum est, convallis at vehicula venenatis, convallis vitae nulla. Morbi tempus sed sapien nec sodales. Rank:{_testClasses[i].Rank} Donec tellus nunc, fringilla eu dolor sit amet, eleifend efficitur sem. Vestibulum non diam tortor. Location:{_testClasses[i].Location} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam quis mattis mauris. Morbi ac ultrices dolor State:{_testClasses[i].State} Duis id consectetur lacus. Fusce ac neque at odio interdum commodo. Quisque ac magna metus. Maecenas sed lacinia odio Country:{_testClasses[i].Country} Aenean at odio volutpat, molestie felis id, vestibulum tellus. Vivamus pellentesque ut urna iaculis vehicula.";
            }
        }
        
        [Benchmark]
        public void SubmergeFromObject()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
        
            for (var i = 0; i < _iterations; i++)
            {
                config.UpdateOrAddFromObject(_testClasses[i]);
                
                submergeTokenReplacer.Replace(_testString);
            }
        }
        
        [Benchmark]
        public void SubmergeUpdateOrAddMapping()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            for (var i = 0; i < _iterations; i++)
            {
                config.UpdateOrAddMapping("name", _testClasses[i].Name);
                config.UpdateOrAddMapping("age", _testClasses[i].Age);
                config.UpdateOrAddMapping("rank", _testClasses[i].Rank);
                config.UpdateOrAddMapping("location", _testClasses[i].Location);
                config.UpdateOrAddMapping("state", _testClasses[i].State);
                config.UpdateOrAddMapping("country", _testClasses[i].Country);
        
                submergeTokenReplacer.Replace(_testString);
            }
        }
        
        [Benchmark]
        public void SubmergeReplaceEnumerable()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var subMaps = new List<ISubstitutionMap>();
            
            for (var i = 0; i < _iterations; i++)
            {
                var substitutionMap = new SubstitutionMap()
                    .UpdateOrAddMapping("name", _testClasses[i].Name)
                    .UpdateOrAddMapping("age", _testClasses[i].Age)
                    .UpdateOrAddMapping("rank", _testClasses[i].Rank)
                    .UpdateOrAddMapping("location", _testClasses[i].Location)
                    .UpdateOrAddMapping("state", _testClasses[i].State)
                    .UpdateOrAddMapping("country", _testClasses[i].Country);
        
                subMaps.Add(substitutionMap);
            } 
            
            var result = submergeTokenReplacer.Replace(_testString, subMaps).ToList();
        }
        
        [Benchmark]
        public void SubmergeReplaceWithGetMatches()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var matches = submergeTokenReplacer.GetMatches(_testString);
            
            for (var i = 0; i < _iterations; i++)
            {
                var substitutionMap = new SubstitutionMap()
                    .UpdateOrAddMapping("name", _testClasses[i].Name)
                    .UpdateOrAddMapping("age", _testClasses[i].Age)
                    .UpdateOrAddMapping("rank", _testClasses[i].Rank)
                    .UpdateOrAddMapping("location", _testClasses[i].Location)
                    .UpdateOrAddMapping("state", _testClasses[i].State)
                    .UpdateOrAddMapping("country", _testClasses[i].Country);
        
                submergeTokenReplacer.Replace(matches, substitutionMap);
            }
        }
        
        [Benchmark]
        public void SubmergeReplaceWithGetMatchesReplaceSubstitutions()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var matches = submergeTokenReplacer.GetMatches(_testString);
            var substitutionMap = new SubstitutionMap();
            
            for (var i = 0; i < _iterations; i++)
            {
                substitutionMap
                    .UpdateOrAddMapping("name", _testClasses[i].Name)
                    .UpdateOrAddMapping("age", _testClasses[i].Age)
                    .UpdateOrAddMapping("rank", _testClasses[i].Rank)
                    .UpdateOrAddMapping("location", _testClasses[i].Location)
                    .UpdateOrAddMapping("state", _testClasses[i].State)
                    .UpdateOrAddMapping("country", _testClasses[i].Country);
        
                submergeTokenReplacer.Replace(matches, substitutionMap);
            }
        }
        
        [Benchmark]
        public void SubmergeReplaceWithGetMatchesPrecalculatedSubMap()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var matches = submergeTokenReplacer.GetMatches(_testString);
            
            for (var i = 0; i < _iterations; i++)
            {
                submergeTokenReplacer.Replace(matches, _substitutionMaps[i]);
            }
        }
        
        
        [Benchmark]
        public void SubmergeReplaceWithGetMatchesTokenReplacementSet()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var matches = submergeTokenReplacer.GetFixedMatches(_testString);
            
            for (var i = 0; i < _iterations; i++)
            {
                using var tokenReplacementSet = new TokenReplacementSet()
                    .AddReplacement(_testClasses[i].Name)
                    .AddReplacement(_testClasses[i].Age)
                    .AddReplacement(_testClasses[i].Rank)
                    .AddReplacement(_testClasses[i].Location)
                    .AddReplacement(_testClasses[i].State)
                    .AddReplacement(_testClasses[i].Country);
                
                submergeTokenReplacer.Replace(matches, tokenReplacementSet);
            }
        }

        [Benchmark]
        public void SubmergeReplaceWithGetMatchesTokenReplacementSetPreCalculated()
        {
            for (var i = 0; i < _iterations; i++)
            {
                _submergeTokenReplacer.Replace(_fixedTokenMatchSet, _tokenReplacementSets[i]);
            }
        }
    }
}