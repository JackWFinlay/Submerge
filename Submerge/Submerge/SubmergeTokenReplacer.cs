using System;
using System.Collections.Generic;
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
        
        public FixedTokenMatchSet GetFixedMatches(string input) => _replacementEngine.GetFixedTokenMatchSet(input.AsMemory());


        public string Replace(TokenMatchSet matches, ISubstitutionMap substitutionMap)
        {
            var replaceResult = _replacementEngine.Replace(matches, substitutionMap);

            return replaceResult;
        }

        public string Replace(FixedTokenMatchSet matches, TokenReplacementSet tokenReplacementSet) =>
            _replacementEngine.Replace(matches, tokenReplacementSet);

        private IEnumerable<string> Replace(ReadOnlyMemory<char> input,
            IList<ISubstitutionMap> substitutionMaps)
        {
            var cachedRawString = string.Empty;
            
            var results = _replacementEngine.Replace(input, substitutionMaps);

            foreach (var result in results)
            {
                string escapedString;
                
                if (string.IsNullOrWhiteSpace(result))
                {
                    if (string.IsNullOrWhiteSpace(cachedRawString))
                    {
                        // Only create the string when we actually need it.
                        cachedRawString = input.ToString();
                    }

                    escapedString = cachedRawString;
                }
                else
                {
                    escapedString = result;
                }
                
                yield return escapedString;
            }
        }

        private string Replace(ReadOnlyMemory<char> input)
        {
            var replaceResult = _replacementEngine.Replace(input);

            return replaceResult;
        }
    }
}