using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Submerge.Configuration;
using Submerge.Test;
using Xunit;

namespace Submerge.Tests
{
    public class TokenEscapeTests
    {
        [Fact]
        public async Task TestTokenReplacementEngine_MatchingStartEndTokens_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = substitution;
            const string testString = "{key}";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MatchingStartEndTokensTextAtStart_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution";
            const string testString = "something {key}";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MatchingStartEndTokensTextAtEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something";
            const string testString = "{key} something";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MatchingStartEndTokensTextAtStartAndEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution something";
            const string testString = "something {key} something";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_LongerTokenStartEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = substitution;
            const string testString = "{{key}}";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_LongerTokenStartEndTextAtStart_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution";
            const string testString = "something {{key}}";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_LongerTokenStartEndTextAtEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something";
            const string testString = "{{key}} something";

            var actual = await submergeTokenReplacer
            .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_DoubleEscaped_ReturnsOriginalText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string testString = "something {{key}} something";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(testString);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_DoubleEscapedLongToken_ReturnsOriginalText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string testString = "something *|*|key|*|* something";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(testString);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_LongerTokenStartEndTextAtStartAndEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution something";
            const string testString = "something {{key}} something";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MixedTokenStartEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = substitution;
            const string testString = "*|key|*";

            var actual = await submergeTokenReplacer
                .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MixedTokenStartEndTextAtStart_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution";
            const string testString = "something *|key|*";

            var actual = await submergeTokenReplacer
            .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MixedTokenStartEndTextAtEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something";
            const string testString = "*|key|* something";

            var actual = await submergeTokenReplacer
            .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MixedTokenStartEndTextAtStartAndEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution something";
            const string testString = "something *|key|* something";

            var actual = await submergeTokenReplacer
            .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MixedTokenStartEndMultipleSubstitution_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something substitution something substitution";
            const string testString = "*|key|* something *|key|* something *|key|*";

            var actual = await submergeTokenReplacer
            .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_MixedTokenStartEndMultipleKeys_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .AddMapping("key2", "substitution2")
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something substitution2 something substitution";
            const string testString = "*|key|* something *|key2|* something *|key|*";

            var actual = await submergeTokenReplacer
            .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public async Task TestTokenReplacementEngine_UnmatchedToken_DoesNotMatchToken()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .AddMapping("key2", "substitution2")
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "*|key| something *|key2|* something substitution";
            const string testString = "*|key| something *|key2|* something *|key|*";

            var actual = await submergeTokenReplacer
            .ReplaceAsync(testString);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }

        [Fact]
        public void TestTokenReplacementEngine_ReplaceAsyncEnumerable_ReturnsProperlySubstitutedTextList()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var testMemory = Constants.ExampleMergeTemplate.AsMemory();
            var subMaps = new List<IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>>();
            var iterations = 2;
            var testClasses = new TestClass[iterations];
            
            const string name = "John";
            const string location = "Melbourne";
            const string state = "Victoria";
            const string country = "Australia";
            const int age = 99;
            
            for (var i = 0; i < iterations; i++)
            {
                testClasses[i] = new TestClass()
                {
                    Name = name,
                    Location = location,
                    State = state,
                    Country = country,
                    Age = age,
                    Rank = i
                };
            }
            
            for (var i = 0; i < iterations; i++)
            {
                var subConfig = new TokenReplacementConfigurationBuilder()
                    .SetTokenStart("{")
                    .SetTokenEnd("}")
                    .AddMapping("name", testClasses[i].Name)
                    .AddMapping("age", testClasses[i].Age.ToString())
                    .AddMapping("rank", testClasses[i].Rank.ToString())
                    .AddMapping("location", testClasses[i].Location)
                    .AddMapping("state", testClasses[i].State)
                    .AddMapping("country", testClasses[i].Country)
                    .Build();
                
                subMaps.Add(subConfig.SubstitutionMap);
            }

            var result = submergeTokenReplacer.ReplaceAsync(testMemory, subMaps).ToList();
            
            for (var i = 0; i < result.Count; i++)
            {
                result[i].Should()
                    .Be(
                        $"Name:{name} Age:{age.ToString()} Rank:{testClasses[i].Rank.ToString()} Location:{location} State:{state} Country:{country}");
            }
        }
        
        private class TestClass
        {
            public string Name { get; set; }
            public string Location { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public int Age { get; set; }
            public int Rank { get; set; }
            
        }
    }
}