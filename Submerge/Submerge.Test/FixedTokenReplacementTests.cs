using FluentAssertions;
using Submerge.Abstractions.Models;
using Submerge.Configuration;
using Xunit;

namespace Submerge.Test
{
    public class FixedTokenReplacementTests
    {
        [Fact]
        public void TestTokenReplacementEngine_MatchingStartEndTokens_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            const string expected = substitution;
            const string testString = "{key}";
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MatchingStartEndTokensTextAtStart_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution";
            const string testString = "something {key}";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MatchingStartEndTokensTextAtEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something";
            const string testString = "{key} something";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MatchingStartEndTokensTextAtStartAndEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution something";
            const string testString = "something {key} something";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_LongerTokenStartEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = substitution;
            const string testString = "{{key}}";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_LongerTokenStartEndTextAtStart_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution";
            const string testString = "something {{key}}";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_LongerTokenStartEndTextAtEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something";
            const string testString = "{{key}} something";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }

        [Fact]
        public void TestTokenReplacementEngine_LongerTokenStartEndTextAtStartAndEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{{")
                .SetTokenEnd("}}")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution something";
            const string testString = "something {{key}} something";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MixedTokenStartEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = substitution;
            const string testString = "*|key|*";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MixedTokenStartEndTextAtStart_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution";
            const string testString = "something *|key|*";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MixedTokenStartEndTextAtEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something";
            const string testString = "*|key|* something";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MixedTokenStartEndTextAtStartAndEnd_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "something substitution something";
            const string testString = "something *|key|* something";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet().AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MixedTokenStartEndMultipleSubstitution_ReturnsProperlySubstitutedText()
        {
            const string substitution = "substitution";
            
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
                .SetTokenEnd("|*")
                .AddMapping("key", substitution)
                .Build();

            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
            const string expected = "substitution something substitution something substitution";
            const string testString = "*|key|* something *|key|* something *|key|*";

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet()
                .AddReplacement(substitution)
                .AddReplacement(substitution)
                .AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }
        
        [Fact]
        public void TestTokenReplacementEngine_MixedTokenStartEndMultipleKeys_ReturnsProperlySubstitutedText()
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

            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            var tokenReplacementSet = new TokenReplacementSet()
                .AddReplacement(substitution)
                .AddReplacement("substitution2")
                .AddReplacement(substitution);
            
            var actual = submergeTokenReplacer
                .Replace(matches, tokenReplacementSet);

            actual.Should()
                .NotBeNull()
                .And
                .Be(expected);
        }

        [Fact]
        public void TestTokenReplacementEngine_ReplaceGetMatches_ReturnsProperlySubstitutedTextList()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            const string testString = Constants.ExampleMergeTemplate;
            var matches = submergeTokenReplacer.GetMatches(testString);
            
            const int iterations = 2;
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
            
            var result = new string[iterations];
            
            for (var i = 0; i < iterations; i++)
            {
                var subMap = new SubstitutionMap()
                    .UpdateOrAddMapping("name", testClasses[i].Name)
                    .UpdateOrAddMapping("age", testClasses[i].Age.ToString())
                    .UpdateOrAddMapping("rank", testClasses[i].Rank.ToString())
                    .UpdateOrAddMapping("location", testClasses[i].Location)
                    .UpdateOrAddMapping("state", testClasses[i].State)
                    .UpdateOrAddMapping("country", testClasses[i].Country);
                
                result[i] = submergeTokenReplacer.Replace(matches, subMap);
            }

            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should()
                    .Be(
                        $"Name:{name} Age:{age.ToString()} Rank:{testClasses[i].Rank.ToString()} Location:{location} State:{state} Country:{country}");
            }
        }
        
        [Fact]
        public void TestTokenReplacementEngine_ReplaceGetMatchesTokenReplacementSet_ReturnsProperlySubstitutedTextList()
        {
            var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
                .SetTokenEnd("}")
                .Build();
            
            var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            const string testString = Constants.ExampleMergeTemplate;
            var matches = submergeTokenReplacer.GetFixedMatches(testString);
            
            const int iterations = 2;
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
            
            var result = new string[iterations];
            
            for (var i = 0; i < iterations; i++)
            {
                var tokenReplacementSet = new TokenReplacementSet()
                    .AddReplacement(testClasses[i].Name)
                    .AddReplacement(testClasses[i].Age.ToString())
                    .AddReplacement(testClasses[i].Rank.ToString())
                    .AddReplacement(testClasses[i].Location)
                    .AddReplacement(testClasses[i].State)
                    .AddReplacement(testClasses[i].Country);
                
                result[i] = submergeTokenReplacer.Replace(matches, tokenReplacementSet);
            }

            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should()
                    .Be(
                        $"Name:{name} Age:{age.ToString()} Rank:{testClasses[i].Rank.ToString()} Location:{location} State:{state} Country:{country}");
            }
        }
    }
}