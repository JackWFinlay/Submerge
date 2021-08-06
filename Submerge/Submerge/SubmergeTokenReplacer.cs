using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Submerge.Abstractions.Interfaces;
using Submerge.Abstractions.Models;
using Submerge.ReplacementEngines;

namespace Submerge
{
    public class SubmergeTokenReplacer
    {
        private readonly IReplacementEngine _replacementEngine;
        
        public SubmergeTokenReplacer(ITokenReplacementConfiguration config)
        {
            _replacementEngine = new TokenReplacementEngine(config);
        }

        public string Replace(string input) => Replace(input.AsMemory());

        public IEnumerable<string> Replace(string input,
            IEnumerable<ISubstitutionMap> substitutionMaps) =>
                 Replace(input.AsMemory(), (IList<ISubstitutionMap>) substitutionMaps);

        public TokenMatchSet GetMatches(string input) => _replacementEngine.GetTokenMatchSet(input.AsMemory());

        public string Replace(TokenMatchSet matches, ISubstitutionMap substitutionMap)
        {
            var replaceResult = _replacementEngine.Replace(matches, substitutionMap);

            var result = CreateString(replaceResult);

            return result;
        }

        private IEnumerable<string> Replace(ReadOnlyMemory<char> input,
            IList<ISubstitutionMap> substitutionMaps)
        {
            var inputAsReplaceResult = new ReplaceResult();
            var cachedUnreplacedString = string.Empty;
            
            var results = _replacementEngine.Replace(input, substitutionMaps);

            foreach (var result in results)
            {
                var escapedString = string.Empty;
                
                if (result.Memory.IsEmpty)
                {
                    if (inputAsReplaceResult.Memory.IsEmpty)
                    {
                        // Cache the empty result only when it actually needs to be generated.
                        var memory = new ReadOnlyMemory<ReadOnlyMemory<char>>(new[] {input});

                        inputAsReplaceResult = new ReplaceResult()
                        {
                            Memory = memory,
                            Length = input.Length
                        };
                    }

                    if (string.IsNullOrWhiteSpace(escapedString))
                    {
                        // Only create the string when we actually need it.
                        cachedUnreplacedString = CreateString(inputAsReplaceResult);
                    }

                    escapedString = cachedUnreplacedString;
                }
                else
                {
                    escapedString = CreateString(result);
                }
                
                yield return escapedString;
            }
        }

        private string Replace(ReadOnlyMemory<char> input)
        {
            var replaceResult = _replacementEngine.Replace(input);

            if (replaceResult.Memory.IsEmpty)
            {
                var memory = new ReadOnlyMemory<ReadOnlyMemory<char>>(new[] { input });

                replaceResult = new ReplaceResult()
                {
                    Memory = memory,
                    Length = input.Length
                };
            }

            var escaped = CreateString(replaceResult);

            return escaped;
        }

        private static string CreateString(ReplaceResult input)
        {
#if NETSTANDARD2_0
            var stringBuilder = new StringBuilder();
            var span = input.Span;
            
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < span[i].Length; i++)
                {
                    stringBuilder.Append(span[i].Span[j]);
                }
            }
            var escaped = stringBuilder.ToString();
#else
                var escaped = string.Create(input.Length, input.Memory,
                (destination, state) =>
                {
                    var prevLength = 0;
                    for (var i = 0; i < state.Length; i++)
                    {
                        state.Span[i].Span.CopyTo(destination.Slice(prevLength));
                        prevLength += state.Span[i].Length;
                    }
                });
#endif
            return escaped;
        }
    }
}