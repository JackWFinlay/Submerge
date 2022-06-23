using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Submerge.Abstractions.Interfaces;
using Submerge.Configuration;
using Xunit;

namespace Submerge.Test;

public class TokenReplacementTests
{
    [Fact]
    public void TestTokenReplacementEngine_MatchingStartEndTokens_ReturnsProperlySubstitutedText()
    {
        const string substitution = "substitution";
            
        var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
            .SetTokenEnd("}")
            .AddMapping("key", substitution)
            .Build();

        var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
        const string expected = substitution;
        const string testString = "{key}";

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

        actual.Should()
            .NotBeNull()
            .And
            .Be(expected);
    }
        
    [Fact]
    public void TestTokenReplacementEngine_DoubleEscaped_ReturnsOriginalText()
    {
        const string substitution = "substitution";
            
        var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
            .SetTokenEnd("}")
            .AddMapping("key", substitution)
            .Build();

        var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
        const string testString = "something {{key}} something";

        var actual = submergeTokenReplacer
            .Replace(testString);

        actual.Should()
            .NotBeNull()
            .And
            .Be(testString);
    }
        
    [Fact]
    public void TestTokenReplacementEngine_DoubleEscapedLongToken_ReturnsOriginalText()
    {
        const string substitution = "substitution";
            
        var config = new TokenReplacementConfigurationBuilder().SetTokenStart("*|")
            .SetTokenEnd("|*")
            .AddMapping("key", substitution)
            .Build();

        var submergeTokenReplacer = new SubmergeTokenReplacer(config);
            
        const string testString = "something *|*|key|*|* something";

        var actual = submergeTokenReplacer
            .Replace(testString);

        actual.Should()
            .NotBeNull()
            .And
            .Be(testString);
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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

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

        var actual = submergeTokenReplacer
            .Replace(testString);

        actual.Should()
            .NotBeNull()
            .And
            .Be(expected);
    }
        
    [Fact]
    public void TestTokenReplacementEngine_UnmatchedToken_DoesNotMatchToken()
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

        var actual = submergeTokenReplacer
            .Replace(testString);

        actual.Should()
            .NotBeNull()
            .And
            .Be(expected);
    }

    [Fact]
    public void TestTokenReplacementEngine_ReplaceEnumerable_ReturnsProperlySubstitutedTextList()
    {
        var config = new TokenReplacementConfigurationBuilder().SetTokenStart("{")
            .SetTokenEnd("}")
            .Build();
            
        var submergeTokenReplacer = new SubmergeTokenReplacer(config);
        const string testString = Constants.ExampleMergeTemplate;
        var subMaps = new List<ISubstitutionMap>();

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

        for (var i = 0; i < iterations; i++)
        {
            var subMap = new SubstitutionMap()
                .UpdateOrAddMapping("name", testClasses[i].Name)
                .UpdateOrAddMapping("age", testClasses[i].Age.ToString())
                .UpdateOrAddMapping("rank", testClasses[i].Rank.ToString())
                .UpdateOrAddMapping("location", testClasses[i].Location)
                .UpdateOrAddMapping("state", testClasses[i].State)
                .UpdateOrAddMapping("country", testClasses[i].Country);
                
            subMaps.Add(subMap);
        }

        var results = submergeTokenReplacer.Replace(testString, subMaps).ToList();

        for (var i = 0; i < results.Count; i++)
        {
            results[i].Should()
                .Be(
                    $"Name:{name} Age:{age.ToString()} Rank:{i.ToString()} Location:{location} State:{state} Country:{country}");
        }
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
}