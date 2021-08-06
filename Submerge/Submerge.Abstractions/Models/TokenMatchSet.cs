using System;

namespace Submerge.Abstractions.Models
{
    public struct TokenMatchSet
    {
        public ReadOnlyMemory<char> Input { get; init; }
        public ReadOnlyMemory<TokenMatch> TokenMatches { get; init; }
    }
}