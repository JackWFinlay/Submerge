using System;

namespace Submerge.Abstractions.Models
{
    public class TokenMatchSet
    {
        public ReadOnlyMemory<char> Input { get; init; }
        public ValueList<TokenMatch> TokenMatches { get; init; }
    }
}