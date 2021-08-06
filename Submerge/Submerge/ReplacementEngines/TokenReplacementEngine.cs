using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Submerge.Abstractions.Exceptions;
using Submerge.Abstractions.Interfaces;
using Submerge.Abstractions.Models;
using Submerge.Configuration;
using Submerge.DataStructures;
using Submerge.Extensions;

namespace Submerge.ReplacementEngines
{
    public class TokenReplacementEngine : IReplacementEngine
    {
        private readonly ITokenReplacementConfiguration _config;

        public TokenReplacementEngine(ITokenReplacementConfiguration config)
        {
            _config = config;
        }

        public ReplaceResult Replace(ReadOnlyMemory<char> raw)
        {
            if (raw.IsEmpty)
            {
                return new ReplaceResult();
            }
            
            try
            {
                var replaceResult = CreateEscapedTextList(raw);
                
                if (replaceResult.Memory.IsEmpty)
                {
                    return new ReplaceResult();
                }

                return replaceResult;
            }
            catch (Exception e)
            {
                throw new SubmergeParseException($"Cannot parse the string '{raw.ToString()}'", e);
            }
        }

        public IEnumerable<ReplaceResult> Replace(ReadOnlyMemory<char> raw,
            IList<ISubstitutionMap> substitutionMaps)
        {
            if (!substitutionMaps.Any())
            {
                yield break;
            }

            var matchIndexList = GetMatches(raw);

            foreach (var substitutionMap in substitutionMaps)
            {
                var result = ReplaceWithMatchIndexList(raw, matchIndexList, substitutionMap);

                yield return result;
            }
        }

        public ReplaceResult Replace(TokenMatchSet matchSet, ISubstitutionMap substitutionMap)
        {
            var result = ReplaceWithMatchIndexList(matchSet.Input, matchSet.TokenMatches, substitutionMap);

            return result;
        }

        public TokenMatchSet GetTokenMatchSet(ReadOnlyMemory<char> input)
        {
            var matches = GetMatches(input);

            var result = new TokenMatchSet()
            {
                Input = input,
                TokenMatches = matches
            };

            return result;
        }

        private ReplaceResult CreateEscapedTextList(ReadOnlyMemory<char> raw)
        {
            var result = new ValueList<ReadOnlyMemory<char>>();
            var length = 0;
            var prevIndex = 0;

            while (prevIndex < raw.Length)
            {
                var slice = raw.Slice(prevIndex);

                var indexOfTokenStart = slice.IndexOfTokenStart(_config.TokenStart);

                if (indexOfTokenStart < 0)
                {
                    break;
                }

                if (prevIndex + indexOfTokenStart >= raw.Length)
                {
                    prevIndex = raw.Length;
                    break;
                }
                
                // Add chars preceding token.
                if (indexOfTokenStart > 0)
                {
                    var memorySlice = raw.Slice(prevIndex, indexOfTokenStart);
                    if (!memorySlice.IsEmpty)
                    {
                        result.Add(memorySlice);
                        length += memorySlice.Length;
                    }
                }

                var startOfTokenEndIndex = raw.Slice(prevIndex + indexOfTokenStart + _config.TokenStart.Length).IndexOfTokenStart(_config.TokenEnd);

                if (startOfTokenEndIndex < 0)
                {
                    // Malformed token. Return empty token.
                    return new ReplaceResult();
                }

                // We can just grab the slice of length startOfTokenEndIndex because of zero-based indexing.
                // e.g. where slice = 'abc|*...' with token end '|*' then index of token end is 3, so we take the first 3 chars.
                var token = raw.Slice(prevIndex + indexOfTokenStart + _config.TokenStart.Length, startOfTokenEndIndex);

                if (_config.SubstitutionMap.TryGetValue(token, out var substitution))
                {
                    if (!substitution.IsEmpty)
                    {
                        result.Add(substitution);
                        length += substitution.Length;
                    }
                }
                else
                {
                    // This is a substitution token that isn't registered. Don't replace it.
                    result.Add(_config.TokenStart);
                    result.Add(token);
                    result.Add(_config.TokenEnd);
                    length += (_config.TokenStart.Length + token.Length + _config.TokenEnd.Length);
                }

                // Skip past what we know to be the token.
                var tokenLength = _config.TokenStart.Length + token.Length + _config.TokenEnd.Length;
                if (prevIndex == 0)
                {
                    prevIndex = (indexOfTokenStart + tokenLength);
                }
                else
                {
                    prevIndex += (indexOfTokenStart + tokenLength);
                }
            }
            
            // Add on end of raw.
            if (prevIndex < raw.Length)
            {
                var endSlice = raw.Slice(prevIndex);
                result.Add(endSlice);
                length += endSlice.Length;
            }

            var replaceResult = new ReplaceResult()
            {
                Memory = result.AsMemory(),
                Length = length
            };
            
            return replaceResult;
        }

        private ReadOnlyMemory<TokenMatch> GetMatches(ReadOnlyMemory<char> memory)
        {
            var valueList = new ValueList<TokenMatch>();
            
            while (memory.Length > 0)
            {
                var index = memory.IndexOfTokenStart(_config.TokenStart);
                
                if (index == -1)
                {
                    valueList.Add(new TokenMatch { Index = memory.Length});
                    break;
                }

                // Take a slice without the token start.
                var slice = memory.Slice((index + _config.TokenStart.Length));
                var token = GetToken(slice, _config.TokenEnd);

                var tokenMatch = new TokenMatch
                {
                    Index = index,
                    Token = token
                };
                
                valueList.Add(tokenMatch);

                // Skip past the entire token.
                var newIndex = index + _config.TokenStart.Length + token.Length + _config.TokenEnd.Length;

                // Token is at end of string.
                if (newIndex >= memory.Length)
                {
                    valueList.Add(new TokenMatch { Index = memory.Length});
                    break;
                }

                memory = memory.Slice(newIndex);
            }

            return valueList.AsMemory();
        }

        private static ReadOnlyMemory<char> GetToken(ReadOnlyMemory<char> slice, ReadOnlyMemory<char> configTokenEnd)
        {
            var startOfTokenEndIndex = slice.IndexOfTokenStart(configTokenEnd);

            if (startOfTokenEndIndex < 0)
            {
                // Malformed token. Return empty token.
                return Array.Empty<char>();
            }

            // We can just grab the slice of length startOfTokenEndIndex because of zero-based indexing.
            // e.g. where slice = 'abc|*...' with token end '|*' then index of token end is 3, so we take the first 3 chars.
            var token = slice.Slice(0, startOfTokenEndIndex);

            return token;
        }
        
        private ReplaceResult ReplaceWithMatchIndexList(ReadOnlyMemory<char> raw,
            ReadOnlyMemory<TokenMatch> matches,
            ISubstitutionMap substitutionMap)
        {
            var result = new ValueList<ReadOnlyMemory<char>>();
            var length = 0;
            var prevIndex = 0;

            foreach (var match in matches.Span)
            {
                if (prevIndex >= raw.Length || match.Index >= raw.Length)
                {
                    break;
                }

                if (match.Index > 0)
                {
                    var memorySlice = raw.Slice(prevIndex, match.Index);
                    if (!memorySlice.IsEmpty)
                    {
                        result.Add(memorySlice);
                        length += memorySlice.Length;
                    }
                }
                
                if (prevIndex + match.Index >= raw.Length)
                {
                    break;
                }

                if (substitutionMap.TryGetValue(match.Token, out var substitution))
                {
                    if (!substitution.IsEmpty)
                    {
                        result.Add(substitution);
                        length += substitution.Length;
                    }
                }
                else
                {
                    // This is a substitution token that isn't registered. Don't replace it.
                    result.Add(_config.TokenStart);
                    result.Add(match.Token);
                    result.Add(_config.TokenEnd);
                    length += (_config.TokenStart.Length + match.Token.Length + _config.TokenEnd.Length);
                }

                var tokenLength = _config.TokenStart.Length + match.Token.Length + _config.TokenEnd.Length;
                if (prevIndex == 0)
                {
                    prevIndex = (match.Index + tokenLength);
                }
                else
                {
                    prevIndex += (match.Index + tokenLength);
                }

            }

            var replaceResult = new ReplaceResult
            {
                Memory = result.AsMemory(),
                Length = length
            };
            
            return replaceResult;
        }
    }
}