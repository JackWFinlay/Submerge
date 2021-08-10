using System;
using Submerge.Abstractions.Interfaces;

namespace Submerge.Abstractions.Models
{
    public class TokenMatchSet
    {
        public ReadOnlyMemory<char> Input { get; init; }
        public ValueList<TokenMatch> TokenMatches { get; init; }
    }
}